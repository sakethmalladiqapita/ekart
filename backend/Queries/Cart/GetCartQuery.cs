/// <summary>
/// CQRS Query.
/// Represents a request to retrieve the contents of a user's shopping cart.
/// This query does not modify any system state — it's read-only.
/// </summary>
public class GetCartQuery
{
    /// <summary>
    /// The ID of the user whose cart is being requested.
    /// </summary>
    public string UserId { get; set; }
}
