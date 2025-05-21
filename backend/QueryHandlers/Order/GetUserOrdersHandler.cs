using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;

public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersQuery, List<Order>>
{
    private readonly IMongoCollection<Order> _orders;

    public GetUserOrdersHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
    }

    public async Task<List<Order>> Handle(GetUserOrdersQuery query, CancellationToken cancellationToken)
    {
        return await _orders.Find(o => o.UserId == query.UserId).ToListAsync(cancellationToken);
    }
}