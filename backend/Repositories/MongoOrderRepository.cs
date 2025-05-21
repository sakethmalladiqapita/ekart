using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoOrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public MongoOrderRepository(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _orders = db.GetCollection<Order>(settings.Value.OrderCollection);
    }

    public async Task<Order?> GetByIdAsync(string orderId)
    {
        return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
    }

    public async Task<List<Order>> GetByUserIdAsync(string userId)
    {
        return await _orders.Find(o => o.UserId == userId).ToListAsync();
    }

    public async Task CreateAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
    }

    public async Task UpdateStatusAsync(string orderId, string newStatus)
    {
        var update = Builders<Order>.Update.Set(o => o.Status, newStatus);
        await _orders.UpdateOneAsync(o => o.Id == orderId, update);
    }
}
