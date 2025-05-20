using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ekart.Services
{
    public class UserService : IUserService
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly CreateUserHandler _createUserHandler;
        private readonly AuthenticateUserHandler _authHandler;
        private readonly IMongoCollection<User> _users;

        public UserService(
            ICartService cartService,
            IOrderService orderService,
            CreateUserHandler createUserHandler,
            AuthenticateUserHandler authHandler,
            IMongoClient mongoClient,
            IOptions<DatabaseSettings> settings)
        {
            _cartService = cartService;
            _orderService = orderService;
            _createUserHandler = createUserHandler;
            _authHandler = authHandler;

            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = db.GetCollection<User>(settings.Value.UserCollection);
        }

        // Authenticate a user using CQRS handler
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var command = new AuthenticateUserCommand { Email = email, Password = password };
            return await _authHandler.Handle(command);
        }

        // Create a user using CQRS handler
        public async Task<User> CreateUserAsync(User user)
        {
            var command = new CreateUserCommand { Email = user.Email, Password = user.PasswordHash };
            return await _createUserHandler.Handle(command);
        }

        // Get items in the user's cart
        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _cartService.GetCartAsync(userId);
        }

        // Add item to cart
        public async Task AddToCartAsync(string userId, string productId, int quantity)
        {
            await _cartService.AddToCartAsync(userId, productId, quantity);
        }

        // Checkout the user's cart and create the order
        public async Task<Order> CheckoutCartAsync(string userId)
        {
            var order = await _cartService.CheckoutCartAsync(userId);
            if (order == null)
                throw new Exception("Cart is empty or user not found");

            await _orderService.CreateOrderAsync(order);

            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            user.Orders.Add(new OrderSummary
            {
                OrderId = order.Id,
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
            });

            await _users.ReplaceOneAsync(u => u.Id == userId, user);
            return order;
        }
    }
}
