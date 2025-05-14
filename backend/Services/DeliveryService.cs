using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class DeliveryService : IDeliveryService
{
    private readonly IMongoCollection<Delivery> _deliveries;

    public DeliveryService(IOptions<DatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _deliveries = database.GetCollection<Delivery>(settings.Value.DeliveryCollection);
    }

    public async Task<string> GetStatusAsync(string orderId)
    {
        var delivery = await _deliveries.Find(d => d.OrderId == orderId).FirstOrDefaultAsync();
        return delivery?.DeliveryStatus ?? "Not Shipped";
    }
}
