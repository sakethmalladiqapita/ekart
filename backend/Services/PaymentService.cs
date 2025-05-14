using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Razorpay.Api;

public class PaymentService : IPaymentService
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly string _razorpayKey;
    private readonly string _razorpaySecret;


    public PaymentService(IOptions<DatabaseSettings> dbSettings, IOptions<RazorpaySettings> razorpayOptions)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = database.GetCollection<Payment>(dbSettings.Value.PaymentCollection);

        _razorpayKey = razorpayOptions.Value.Key;
        _razorpaySecret = razorpayOptions.Value.Secret;
    }


    public async Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount)
    {
        var options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) },  // Razorpay expects paise
            { "currency", "INR" },
            { "receipt", orderId }
        };

        var client = new RazorpayClient(_razorpayKey, _razorpaySecret);
        var order = client.Order.Create(options);
        return order.Attributes;
    }

public async Task ConfirmPaymentAsync(ConfirmPaymentRequest request)
{
    // Insert payment record
    var payment = new Payment
    {
        Id = ObjectId.GenerateNewId().ToString(),
        UserId = "",  // Replace with actual userId if available from order
        OrderId = request.OrderId,
        Amount = 0,  // You can update this if Razorpay sends actual amount
        PaymentStatus = "Success",
        PaymentDate = DateTime.UtcNow
    };
    await _payments.InsertOneAsync(payment);

    // Setup MongoDB connection to Orders and Deliveries
    var client = new MongoClient("mongodb://localhost:27017"); // or use injected client
    var database = client.GetDatabase("ekartdatabase");

    var orders = database.GetCollection<Order>("OrdersData");
    var deliveries = database.GetCollection<Delivery>("DeliveriesData");

    // 1. Update order status to "Done"
    var updateOrder = Builders<Order>.Update.Set(o => o.Status, "Done");
    await orders.UpdateOneAsync(o => o.Id == request.OrderId, updateOrder);

    // 2. Insert delivery record
    var delivery = new Delivery
    {
        OrderId = request.OrderId,
        DeliveryStatus = "Processing",
        ExpectedDate = DateTime.UtcNow.AddDays(3)
    };
    await deliveries.InsertOneAsync(delivery);
}


    public async Task<string> GetPaymentStatusAsync(string orderId)
    {
        var payment = await _payments.Find(p => p.OrderId == orderId).FirstOrDefaultAsync();
        return payment?.PaymentStatus ?? "Not Paid";
    }
}