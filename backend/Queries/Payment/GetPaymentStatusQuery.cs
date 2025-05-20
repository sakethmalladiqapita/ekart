/// <summary>
/// CQRS Query.
/// Represents a request to retrieve the current payment status of a specific order.
/// This query does not modify any system state.
/// </summary>
public class GetPaymentStatusQuery
{
    /// <summary>
    /// The ID of the order for which payment status is being queried.
    /// </summary>
    public string OrderId { get; set; }
}
