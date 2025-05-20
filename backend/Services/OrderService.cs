using ekart.Models;

namespace ekart.Services
{
    public class OrderService : IOrderService
    {
        private readonly CreateOrderHandler _createOrderHandler;
        private readonly GetUserOrdersHandler _getUserOrdersHandler;
        private readonly BuyNowHandler _buyNowHandler;

        public OrderService(
            CreateOrderHandler createOrderHandler,
            GetUserOrdersHandler getUserOrdersHandler,
            BuyNowHandler buyNowHandler)
        {
            _createOrderHandler = createOrderHandler;
            _getUserOrdersHandler = getUserOrdersHandler;
            _buyNowHandler = buyNowHandler;
        }

        // Create order using command handler
        public async Task CreateOrderAsync(Order order)
        {
            var command = new CreateOrderCommand
            {
                UserId = order.UserId,
                ProductItems = order.ProductItems,
                TotalAmount = order.TotalAmount
            };

            await _createOrderHandler.Handle(command);
        }

        // Get orders for a user
        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _getUserOrdersHandler.Handle(new GetUserOrdersQuery { UserId = userId });
        }

        // Buy-now operation
        public async Task<Order> BuyNowAsync(string userId, string productId, int quantity)
        {
            var command = new BuyNowCommand
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            return await _buyNowHandler.Handle(command);
        }
    }
}
