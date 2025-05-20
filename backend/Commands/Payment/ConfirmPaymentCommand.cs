/// <summary>
/// CQRS Command.
/// Represents a request to confirm a payment after receiving payment confirmation from Razorpay or another gateway.
/// </summary>
public class ConfirmPaymentCommand
{
    /// <summary>
    /// The ID of the order for which payment confirmation is being processed.
    /// </summary>
    public string OrderId { get; set; }
}
