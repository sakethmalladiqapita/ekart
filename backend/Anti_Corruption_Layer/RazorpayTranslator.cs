using Razorpay.Api;
using MongoDB.Bson;

public class RazorpayTranslator
{
    public Dictionary<string, object> ToExternalRequest(Order order)
    {
        return new Dictionary<string, object>
        {
            { "amount", (int)(order.TotalAmount * 100) },
            { "currency", "INR" },
            { "receipt", order.Id }
        };
    }

    public Payment TranslateToInternalModel(Razorpay.Api.Payment payment)
    {
        return new Payment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            PaymentStatus = payment["status"],
            OrderId = payment["order_id"],
            Amount = (decimal)payment["amount"] / 100,
            PaymentDate = DateTime.UtcNow,
            Signature = payment["signature"]
        };
    }
}
