using ekart.Models;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<List<Order>> GetUserOrdersAsync(); // 🔄 no userId
    Task<Order> BuyNowAsync(string productId, int quantity); // 🔄 no userId
}
