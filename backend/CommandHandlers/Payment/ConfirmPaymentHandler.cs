using MediatR;
using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CommandHandlers{
public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly IMongoCollection<Order> _orders;
    private readonly IMongoCollection<Delivery> _deliveries;

    public ConfirmPaymentHandler(IMongoClient client, IOptions<DatabaseSettings> dbSettings)
    {
        var db = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = db.GetCollection<Payment>(dbSettings.Value.PaymentCollection);
        _orders = db.GetCollection<Order>(dbSettings.Value.OrderCollection);
        _deliveries = db.GetCollection<Delivery>("DeliveriesData");
    }

    public async Task<Unit> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            OrderId = command.OrderId,
            Amount = 0,
            PaymentStatus = "Success",
            PaymentDate = DateTime.UtcNow
        };
        await _payments.InsertOneAsync(payment, cancellationToken: cancellationToken);

        var updateOrder = Builders<Order>.Update.Set(o => o.Status, "Successful");
        await _orders.UpdateOneAsync(o => o.Id == command.OrderId, updateOrder, cancellationToken: cancellationToken);

        var delivery = new Delivery
        {
            OrderId = command.OrderId,
            DeliveryStatus = "Processing",
            ExpectedDate = DateTime.UtcNow.AddDays(3)
        };
        await _deliveries.InsertOneAsync(delivery, cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
}