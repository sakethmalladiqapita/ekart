using ekart.Models;

/// <summary>
/// DDD Repository Interface for the Order aggregate.
/// Encapsulates data access logic and abstracts persistence for Orders.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Fetch a single order by its unique ID.
    /// </summary>
    Task<Order> GetByIdAsync(string id);

    /// <summary>
    /// Get all orders placed by a specific user.
    /// </summary>
    Task<List<Order>> GetByUserIdAsync(string userId);

    /// <summary>
    /// Create and persist a new order.
    /// </summary>
    Task CreateAsync(Order order);

    /// <summary>
    /// Update the status (e.g., to Shipped, Delivered) of an existing order.
    /// </summary>
    Task UpdateStatusAsync(string orderId, string newStatus);
}
