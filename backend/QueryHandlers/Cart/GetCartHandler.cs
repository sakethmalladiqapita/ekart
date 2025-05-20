using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Query Handler.
/// Handles GetCartQuery by retrieving a user's active cart items from the database.
/// This is a read-only operation — no changes to system state.
/// </summary>
public class GetCartHandler
{
    private readonly IMongoCollection<User> _users;

    /// <summary>
    /// Initializes the MongoDB collection used for reading user data.
    /// </summary>
    public GetCartHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    /// <summary>
    /// Retrieves all cart items for the specified user, excluding any with non-positive quantity.
    /// </summary>
    /// <param name="query">Query object containing the UserId</param>
    /// <returns>A list of CartItems, or an empty list if the user/cart is not found</returns>
    public async Task<List<CartItem>> Handle(GetCartQuery query)
    {
        var user = await _users.Find(u => u.Id == query.UserId).FirstOrDefaultAsync();
        return user?.Cart?.Where(c => c.Quantity > 0).ToList() ?? new List<CartItem>();
    }
}
