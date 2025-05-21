using ekart.Models;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class GetDeliveryStatusHandler : IRequestHandler<GetDeliveryStatusQuery, string>
{
    private readonly IMongoCollection<Delivery> _deliveries;

    public GetDeliveryStatusHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _deliveries = db.GetCollection<Delivery>(settings.Value.DeliveryCollection);
    }

    public async Task<string> Handle(GetDeliveryStatusQuery query, CancellationToken cancellationToken)
    {
        var delivery = await _deliveries.Find(d => d.OrderId == query.OrderId)
                                        .FirstOrDefaultAsync(cancellationToken);
        return delivery?.DeliveryStatus ?? "Not Shipped";
    }
}
