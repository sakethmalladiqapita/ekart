using MongoDB.Bson;

public class PaymentCompletedEvent
{
    public string OrderId { get; }
    public string PaymentId { get; }

    public PaymentCompletedEvent(string orderId, string paymentId)
    {
        OrderId = orderId;
        PaymentId = paymentId;
    }
}
