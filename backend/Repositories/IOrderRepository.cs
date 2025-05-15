public interface IOrderRepository
{
    Task<Order> GetByIdAsync(string id);
    Task<List<Order>> GetByUserIdAsync(string userId);
    Task CreateAsync(Order order);
    Task UpdateStatusAsync(string orderId, string newStatus);
}
