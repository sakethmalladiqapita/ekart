public class OrderCreatedEvent
{
    public string OrderId { get; }
    public string UserId { get; }

    public OrderCreatedEvent(string orderId, string userId)
    {
        OrderId = orderId;
        UserId = userId;
    }
}
