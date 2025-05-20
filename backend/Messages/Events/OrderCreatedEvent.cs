namespace Events.Messages
{
    /// <summary>
    /// Domain Event published after an order is successfully created.
    /// Used in a distributed system to notify other services (e.g., Payment, Delivery).
    /// </summary>
    public class OrderCreatedEvent : IEvent
    {
        /// <summary>
        /// Unique identifier of the order.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// ID of the user who placed the order.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Total value of the order (used for payment processing, etc.).
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
