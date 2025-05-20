namespace ekart.Models
{
    // Request model to initiate Razorpay payment
    public class PaymentOrderRequest
    {
        public string OrderId { get; set; } // The order to which the payment relates
        public decimal Amount { get; set; }  // Total amount to be paid
    }
}
