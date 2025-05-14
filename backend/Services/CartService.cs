using MongoDB.Driver;
using Microsoft.Extensions.Options;
namespace ekart.Services
{
    public class CartService : ICartService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Product> _products;

        public CartService(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UserCollection);
            _products = database.GetCollection<Product>(settings.Value.ProductCollection);
        }

        public async Task AddToCartAsync(string userId, string productId, int quantity)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return;

            if (user.Cart == null)
                user.Cart = new List<CartItem>();

            var existingItem = user.Cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity <= 0)
                    user.Cart.Remove(existingItem);
            }
            else if (quantity > 0)
            {
                var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
                if (product == null) return;

                user.Cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            await _users.ReplaceOneAsync(u => u.Id == userId, user);
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user?.Cart?.Where(c => c.Quantity > 0).ToList() ?? new List<CartItem>();
        }

        public async Task<Order?> CheckoutCartAsync(string userId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null || user.Cart == null || !user.Cart.Any()) return null;

            var order = new Order
            {
                UserId = userId,
                ProductItems = user.Cart.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    ProductName = c.ProductName,
                    PriceAtPurchase = c.UnitPrice,
                    Quantity = c.Quantity
                }).ToList(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = user.Cart.Sum(i => i.UnitPrice * i.Quantity),
                Status = "Pending"
            };

            // Clear cart after checkout
            user.Cart.Clear();
            await _users.ReplaceOneAsync(u => u.Id == userId, user);

            return order;
        }
    }
}
