/// <summary>
/// CQRS Query.
/// Represents a request to fetch the delivery status of a specific order.
/// This query is read-only and does not alter any state.
/// </summary>
public class GetDeliveryStatusQuery
{
    /// <summary>
    /// The ID of the order whose delivery status is being requested.
    /// </summary>
    public string OrderId { get; set; }
}
