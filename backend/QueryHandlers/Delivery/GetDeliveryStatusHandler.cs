using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Query Handler.
/// Handles GetDeliveryStatusQuery by retrieving the delivery status of a specific order.
/// This handler performs a read-only operation and does not modify system state.
/// </summary>
public class GetDeliveryStatusHandler
{
    private readonly IMongoCollection<Delivery> _deliveries;

    /// <summary>
    /// Initializes MongoDB collection for delivery data.
    /// </summary>
    public GetDeliveryStatusHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _deliveries = db.GetCollection<Delivery>(settings.Value.DeliveryCollection);
    }

    /// <summary>
    /// Returns the delivery status for the given order ID.
    /// If the delivery is not found, defaults to "Not Shipped".
    /// </summary>
    /// <param name="query">Contains the OrderId to lookup</param>
    /// <returns>Delivery status as a string</returns>
    public async Task<string> Handle(GetDeliveryStatusQuery query)
    {
        var delivery = await _deliveries.Find(d => d.OrderId == query.OrderId).FirstOrDefaultAsync();
        return delivery?.DeliveryStatus ?? "Not Shipped";
    }
}
