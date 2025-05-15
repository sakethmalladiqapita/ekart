// === 1. Remove cart-related logic from UserService and delegate to CartService ===
// === Updated UserService.cs ===

using ekart.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IProductService _products;
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public UserService(IOptions<DatabaseSettings> settings, IProductService productService, IOrderService orderService, ICartService cartService)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _users = database.GetCollection<User>(settings.Value.UserCollection);
        _products = productService;
        _orderService = orderService;
        _cartService = cartService;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var filter = Builders<User>.Filter.Eq("email", email);
        var user = await _users.Find(filter).FirstOrDefaultAsync();
        if (user == null) return null;
        return user.PasswordHash.Trim() == password.Trim() ? user : null;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new Exception("Email and password are required");

        var existingUser = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (existingUser != null)
            throw new Exception("User with this email already exists");

        user.Id = ObjectId.GenerateNewId().ToString();
        user.Orders = new List<OrderSummary>();
        user.Cart = new List<CartItem>();

        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<List<CartItem>> GetCartAsync(string userId)
    {
        return await _cartService.GetCartAsync(userId);
    }

    public async Task AddToCartAsync(string userId, string productId, int quantity)
    {
        await _cartService.AddToCartAsync(userId, productId, quantity);
    }

    public async Task<Order> CheckoutCartAsync(string userId)
    {
        var order = await _cartService.CheckoutCartAsync(userId);
        if (order == null) throw new Exception("Cart is empty or user not found");

        await _orderService.CreateOrderAsync(order);

        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        user.Orders.Add(new OrderSummary
        {
            OrderId = order.Id,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            OrderDate = order.OrderDate
        });

        await _users.ReplaceOneAsync(u => u.Id == userId, user);
        return order;
    }
}
