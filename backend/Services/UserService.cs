using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IProductService _products;
    private readonly IOrderService _orderService;

    public UserService(IOptions<DatabaseSettings> settings, IProductService productService, IOrderService orderService)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _users = database.GetCollection<User>(settings.Value.UserCollection);
        _products = productService;
        _orderService = orderService;
    }

public async Task<User?> AuthenticateAsync(string email, string password)
{
    Console.WriteLine($"Trying login: {email} / {password}");
    var filter = Builders<User>.Filter.Eq("email", email);
var user = await _users.Find(filter).FirstOrDefaultAsync();

    if (user == null)
    {
        Console.WriteLine("No user found.");
        return null;
    }

    Console.WriteLine($"DB password: '{user.PasswordHash}' | Input password: '{password}'");

    // Add trim check
    if (user.PasswordHash.Trim() == password.Trim())
    {
        Console.WriteLine("Password match");
        return user;
    }
    else
    {
        Console.WriteLine("Password mismatch");
        return null;
    }
}
    

    public async Task AddToCartAsync(string userId, string productId, int quantity)
    {
        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        var product = (await _products.GetAllAsync()).FirstOrDefault(p => p.Id == productId);

        if (user != null && product != null)
        {
            var existingItem = user.Cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                user.Cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });

            await _users.ReplaceOneAsync(u => u.Id == userId, user);
        }
    }

public async Task<List<CartItem>> GetCartAsync(string userId)
{
    var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
    var cartItems = new List<CartItem>();

    foreach (var item in user.Cart)
    {
        var product = await _products.GetByIdAsync(item.ProductId);
        if (product == null) continue;

        cartItems.Add(new CartItem
        {
            ProductId = product.Id,
            ProductName = product.Name,
            UnitPrice = product.Price,
            Quantity = item.Quantity,
            ImageUrl = product.ImageUrl // Add this property in Product.cs if not there
        });
    }

    return cartItems;
}


    public async Task<Order> CheckoutCartAsync(string userId)
    {
        var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null || !user.Cart.Any()) throw new Exception("Cart is empty or user not found");

        var order = new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ProductItems = user.Cart.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtPurchase = item.UnitPrice
            }).ToList(),
            TotalAmount = user.Cart.Sum(item => item.UnitPrice * item.Quantity)
        };

        await _orderService.CreateOrderAsync(order);
        user.Cart.Clear();

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