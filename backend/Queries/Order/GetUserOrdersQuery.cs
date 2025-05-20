/// <summary>
/// CQRS Query.
/// Represents a request to retrieve all orders placed by a specific user.
/// This is a read-only query with no side effects.
/// </summary>
public class GetUserOrdersQuery
{
    /// <summary>
    /// The ID of the user whose orders are being retrieved.
    /// </summary>
    public string UserId { get; set; }
}
