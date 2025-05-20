using ekart.Models;

/// <summary>
/// CQRS Command.
/// Represents the intention to create a new order, typically after a cart checkout or buy-now operation.
/// </summary>
public class CreateOrderCommand
{
    /// <summary>
    /// ID of the user placing the order.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The list of order items being purchased.
    /// </summary>
    public List<OrderItem> ProductItems { get; set; }

    /// <summary>
    /// The total monetary value of the order.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
