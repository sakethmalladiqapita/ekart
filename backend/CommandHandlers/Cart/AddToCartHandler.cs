using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;

/// <summary>
/// CQRS Command Handler.
/// Handles logic for adding a product to the user's cart.
/// Reads and writes to MongoDB collections directly (Application Layer).
/// </summary>
namespace  ekart.Handlers{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Unit>
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
        public async Task<Unit> Handle(AddToCartCommand command, CancellationToken cancellationToken)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(command.UserId))
                    throw new Exception("UserId is null or empty");

                var user = await _users.Find(u => u.Id == command.UserId).FirstOrDefaultAsync(cancellationToken);
                if (user == null)
                    throw new Exception($"User not found with ID {command.UserId}");

                user.Cart ??= new List<CartItem>();

                var item = user.Cart.FirstOrDefault(c => c.ProductId == command.ProductId);
                if (item != null)
                {
                    item.Quantity += command.Quantity;
                    if (item.Quantity <= 0)
                        user.Cart.Remove(item);
                }
                else if (command.Quantity > 0)
                {
                    var product = await _products.Find(p => p.Id == command.ProductId).FirstOrDefaultAsync(cancellationToken);
                    if (product == null)
                        throw new Exception($"Product not found with ID {command.ProductId}");

                    user.Cart.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        UnitPrice = product.Price,
                        Quantity = command.Quantity
                    });
                }

                await _users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddToCart ERROR] {ex.Message}");
                throw;
            }
        }
    }

}
