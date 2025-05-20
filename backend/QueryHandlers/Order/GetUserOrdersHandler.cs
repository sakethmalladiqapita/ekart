using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Query Handler.
/// Handles GetUserOrdersQuery by retrieving all orders placed by a specific user.
/// This handler performs a read-only operation and returns a full list of Order aggregates.
/// </summary>
public class GetUserOrdersHandler
{
    private readonly IMongoCollection<Order> _orders;

    /// <summary>
    /// Initializes the MongoDB Order collection reference.
    /// </summary>
    public GetUserOrdersHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
    }

    /// <summary>
    /// Returns all orders associated with the given user ID.
    /// </summary>
    /// <param name="query">The GetUserOrdersQuery containing UserId</param>
    /// <returns>List of orders for the specified user</returns>
    public async Task<List<Order>> Handle(GetUserOrdersQuery query)
    {
        return await _orders.Find(o => o.UserId == query.UserId).ToListAsync();
    }
}
