/// <summary>
/// CQRS Command.
/// Represents a request to create a Razorpay order for a given internal order.
/// This prepares the external payment gateway before the user makes a payment.
/// </summary>
public class CreateRazorpayOrderCommand
{
    /// <summary>
    /// The ID of the internal order for which a Razorpay order should be generated.
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Total amount to be paid, in rupees (not paise).
    /// </summary>
    public decimal Amount { get; set; }
}
