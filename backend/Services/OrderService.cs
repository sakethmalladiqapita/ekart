using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OrderService : IOrderService
{
    private readonly IMongoCollection<Order> _orders;
    private readonly IProductService _products;

    public OrderService(IOptions<DatabaseSettings> settings,IProductService productService)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
        _products = productService;
    }

    public async Task<Order> BuyNowAsync(string userId, string productId, int quantity)
    {
        // Fetch product details
        var product = await _products.GetByIdAsync(productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        var price = product.Price;
        var total = price * quantity;

        var order = new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ProductItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    PriceAtPurchase = price
                }
            },
            TotalAmount = total
        };

        await _orders.InsertOneAsync(order);
        return order;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
    }

    public async Task<List<Order>> GetUserOrdersAsync(string userId)
    {
        return await _orders.Find(o => o.UserId == userId).ToListAsync();
    }
}