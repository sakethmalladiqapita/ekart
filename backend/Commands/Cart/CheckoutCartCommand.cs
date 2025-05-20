/// <summary>
/// CQRS Command.
/// Represents an instruction to checkout the user's cart and create an order.
/// This initiates domain behavior to finalize the purchase.
/// </summary>
public class CheckoutCartCommand
{
    /// <summary>
    /// The ID of the user whose cart is being checked out.
    /// </summary>
    public string UserId { get; set; }
}
