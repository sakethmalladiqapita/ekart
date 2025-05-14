public interface IOrderService
{
    Task<Order> BuyNowAsync(string userId, string productId, int quantity);
    Task<List<Order>> GetUserOrdersAsync(string userId);
        Task CreateOrderAsync(Order order);
}
