using ekart.Models;

namespace ekart.Services
{
    public interface IOrderService
    {
        // Immediately place an order for a single product
        Task<Order> BuyNowAsync(string userId, string productId, int quantity);

        // Get list of orders placed by a user
        Task<List<Order>> GetUserOrdersAsync(string userId);

        // Save a new order
        Task CreateOrderAsync(Order order);
    }
}
