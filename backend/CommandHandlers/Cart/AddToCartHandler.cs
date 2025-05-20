using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles logic for adding a product to the user's cart.
/// Reads and writes to MongoDB collections directly (Application Layer).
/// </summary>
public class AddToCartHandler
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Product> _products;

    public AddToCartHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
        _products = db.GetCollection<Product>(settings.Value.ProductCollection);
    }

    /// <summary>
    /// Handles the AddToCartCommand by updating the user's cart.
    /// Increases quantity if item exists; adds new item if not; removes item if quantity goes to zero.
    /// </summary>
    public async Task Handle(AddToCartCommand command)
    {
        var user = await _users.Find(u => u.Id == command.UserId).FirstOrDefaultAsync();
        if (user == null) return;

        user.Cart ??= new List<CartItem>();

        var item = user.Cart.FirstOrDefault(c => c.ProductId == command.ProductId);
        if (item != null)
        {
            item.Quantity += command.Quantity;
            if (item.Quantity <= 0)
                user.Cart.Remove(item); // Remove if quantity is zero or below
        }
        else if (command.Quantity > 0)
        {
            var product = await _products.Find(p => p.Id == command.ProductId).FirstOrDefaultAsync();
            if (product == null) return;

            user.Cart.Add(new CartItem
            {
                ProductId = command.ProductId,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = command.Quantity
            });
        }

        await _users.ReplaceOneAsync(u => u.Id == command.UserId, user); // Persist changes
    }
}
