using ekart.Models;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ekart.Handlers
{
    public class CheckoutCartHandler : IRequestHandler<CheckoutCartCommand, Order?>
    {
        private readonly IMongoCollection<User> _users;

        public CheckoutCartHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
        {
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _users = db.GetCollection<User>(settings.Value.UserCollection);
        }

        public async Task<Order?> Handle(CheckoutCartCommand command, CancellationToken cancellationToken)
        {
            var user = await _users.Find(u => u.Id == command.UserId).FirstOrDefaultAsync(cancellationToken);
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

            user.Cart.Clear();
            await _users.ReplaceOneAsync(u => u.Id == command.UserId, user, cancellationToken: cancellationToken);
            return order;
        }
    }
}