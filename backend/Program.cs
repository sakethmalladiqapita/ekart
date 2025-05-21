using ekart.Services;
using ekart.Models;
using ekart.Middleware;
using CommandHandlers;
using Events.Messages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using ekart.Handlers;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- JWT Key Validation ----------------------
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT secret key is missing.");

// ---------------------- Configuration Binding ----------------------
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<RazorpaySettings>(
    builder.Configuration.GetSection("Razorpay"));

// ---------------------- MongoDB Client ----------------------
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// ---------------------- Domain Factory ----------------------
builder.Services.AddSingleton<OrderFactory>();

// ---------------------- Application Services ----------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ICartService, CartService>();

// ---------------------- CQRS Command + Query Handlers ----------------------
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetUserOrdersHandler>();
builder.Services.AddScoped<BuyNowHandler>();
builder.Services.AddScoped<AddToCartHandler>();
builder.Services.AddScoped<GetCartHandler>();
builder.Services.AddScoped<CheckoutCartHandler>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<AuthenticateUserHandler>();
builder.Services.AddScoped<CreateRazorpayOrderHandler>();
builder.Services.AddScoped<ConfirmPaymentHandler>();
builder.Services.AddScoped<GetPaymentStatusHandler>();
builder.Services.AddScoped<GetDeliveryStatusHandler>();

// ---------------------- NServiceBus Configuration ----------------------
builder.Host.UseNServiceBus(context =>
{
    var endpointConfig = new EndpointConfiguration("PaymentService");

    // Use JSON for serialization
    endpointConfig.UseSerialization<NewtonsoftJsonSerializer>();

    // Development transport (file-based queue)
    var transport = endpointConfig.UseTransport<LearningTransport>();
    transport.Routing().RouteToEndpoint(
        assembly: typeof(OrderCreatedEvent).Assembly,
        destination: "PaymentService");

    endpointConfig.SendFailedMessagesTo("error");
    endpointConfig.AuditProcessedMessagesTo("audit");

    return endpointConfig;
});

// ---------------------- MVC & JSON Settings ----------------------
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// ---------------------- JWT Authentication ----------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ---------------------- Authorization + Swagger + CORS ----------------------
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------- MediatR for CQRS ----------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(AddToCartHandler).Assembly);

var app = builder.Build();

// ---------------------- Middleware Pipeline ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handler
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
