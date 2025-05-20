/// <summary>
/// CQRS Command.
/// Represents a "Buy Now" action where a user directly purchases a single product without using the cart.
/// </summary>
public class BuyNowCommand
{
    /// <summary>
    /// ID of the user initiating the purchase.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// ID of the product to buy.
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// Quantity of the product to purchase.
    /// </summary>
    public int Quantity { get; set; }
}
