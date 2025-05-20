using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles ConfirmPaymentCommand to mark a payment as successful,
/// update order status, and initiate delivery.
/// </summary>
public class ConfirmPaymentHandler
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly IMongoCollection<Order> _orders;
    private readonly IMongoCollection<Delivery> _deliveries;

    public ConfirmPaymentHandler(IMongoClient client, IOptions<DatabaseSettings> dbSettings)
    {
        var db = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = db.GetCollection<Payment>(dbSettings.Value.PaymentCollection);
        _orders = db.GetCollection<Order>(dbSettings.Value.OrderCollection);
        _deliveries = db.GetCollection<Delivery>("DeliveriesData"); // delivery collection name override
    }

    /// <summary>
    /// Handles a successful payment by:
    /// 1. Creating a Payment record.
    /// 2. Updating the Order status.
    /// 3. Initiating a Delivery record.
    /// </summary>
    public async Task Handle(ConfirmPaymentCommand command)
    {
        // Insert successful payment record
        var payment = new Payment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            OrderId = command.OrderId,
            Amount = 0, // Amount can be updated from Razorpay response if needed
            PaymentStatus = "Success",
            PaymentDate = DateTime.UtcNow
        };
        await _payments.InsertOneAsync(payment);

        // Update corresponding order status to Successful
        var updateOrder = Builders<Order>.Update.Set(o => o.Status, "Successful");
        await _orders.UpdateOneAsync(o => o.Id == command.OrderId, updateOrder);

        // Create new delivery record linked to order
        var delivery = new Delivery
        {
            OrderId = command.OrderId,
            DeliveryStatus = "Processing",
            ExpectedDate = DateTime.UtcNow.AddDays(3)
        };
        await _deliveries.InsertOneAsync(delivery);
    }
}
