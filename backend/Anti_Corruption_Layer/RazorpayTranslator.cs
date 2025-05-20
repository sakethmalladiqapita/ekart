using ekart.Models;
using MongoDB.Bson;

/// <summary>
/// Anti-Corruption Layer (ACL) between the Domain Model and the external Razorpay API.
/// Responsible for translating between internal Order/Payment models and Razorpay's expected/requested format.
/// </summary>
public class RazorpayTranslator
{
    /// <summary>
    /// Translates a domain Order object into the external format required by Razorpay.
    /// </summary>
    //<param name="order">The internal domain Order aggregate</param>
    /// <returns>Dictionary that matches Razorpay's expected Create Order parameters</returns>
    public Dictionary<string, object> ToExternalRequest(Order order)
    {
        return new Dictionary<string, object>
        {
            { "amount", (int)(order.TotalAmount * 100) },  // Razorpay expects amount in paise
            { "currency", "INR" },
            { "receipt", order.Id }  // Used for tracking in Razorpay dashboard
        };
    }

    /// <summary>
    /// Translates a Razorpay.Payment object into the internal domain Payment entity.
    /// This shields the rest of the system from Razorpay's API shape and terminology.
    /// </summary>
    // <param name="payment">Razorpay payment object returned by API</param>
    /// <returns>Internal Payment domain model</returns>
    public Payment TranslateToInternalModel(Razorpay.Api.Payment payment)
    {
        return new Payment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            PaymentStatus = payment["status"],
            OrderId = payment["order_id"],
            Amount = (decimal)payment["amount"] / 100, // Convert from paise to rupees
            PaymentDate = DateTime.UtcNow,
            Signature = payment["signature"]
        };
    }
}
