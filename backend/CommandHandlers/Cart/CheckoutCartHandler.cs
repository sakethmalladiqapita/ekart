using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles logic for checking out the user's cart and creating an Order.
/// Part of the Application Layer — does not persist the order, just returns it for further processing.
/// </summary>
public class CheckoutCartHandler
{
    private readonly IMongoCollection<User> _users;

    public CheckoutCartHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    /// <summary>
    /// Creates an Order from the user's current cart.
    /// Does not save the Order to DB — this is done by the service layer or another handler.
    /// </summary>
    public async Task<Order?> Handle(CheckoutCartCommand command)
    {
        var user = await _users.Find(u => u.Id == command.UserId).FirstOrDefaultAsync();
        if (user?.Cart == null || !user.Cart.Any()) return null;

        var order = new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = command.UserId,
            ProductItems = user.Cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                Quantity = c.Quantity,
                PriceAtPurchase = c.UnitPrice
            }).ToList(),
            TotalAmount = user.Cart.Sum(c => c.Quantity * c.UnitPrice),
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        user.Cart.Clear(); // Clear cart after checkout
        await _users.ReplaceOneAsync(u => u.Id == command.UserId, user);

        return order;
    }
}
