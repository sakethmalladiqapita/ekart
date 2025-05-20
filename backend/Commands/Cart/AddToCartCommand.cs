/// CQRS Command.
/// Represents an instruction to add a specific product to a user's cart.
/// Part of the Application Layer – triggers a change in system state.
public class AddToCartCommand
{
    /// <summary>
    /// The ID of the user performing the action.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The ID of the product being added to the cart.
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// The quantity of the product to add.
    /// </summary>
    public int Quantity { get; set; }
}
