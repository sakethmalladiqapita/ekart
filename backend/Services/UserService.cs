using ekart.Models;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ekart.Services
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IMongoCollection<User> _users;

        public UserService(
            IMediator mediator,
            ICartService cartService,
            IOrderService orderService,
            IMongoClient mongoClient,
            IOptions<DatabaseSettings> settings)
        {
            _mediator = mediator;
            _cartService = cartService;
            _orderService = orderService;
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = db.GetCollection<User>(settings.Value.UserCollection);
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            return await _mediator.Send(new AuthenticateUserCommand { Email = email, Password = password });
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await _mediator.Send(new CreateUserCommand { Email = user.Email, Password = user.PasswordHash });
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _cartService.GetCartAsync(userId);
        }

        public async Task AddToCartAsync(string userId, string productId, int quantity)
        {
            await _cartService.AddToCartAsync(userId, productId, quantity);
        }

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

