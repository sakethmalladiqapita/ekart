namespace ekart.Models
{
    // Request model for confirming payment from Razorpay
    public class ConfirmPaymentRequest
    {
        public string? OrderId { get; set; }             // Local Order ID
        public string RazorpayPaymentId { get; set; }   // Razorpay Payment ID
        public string RazorpayOrderId { get; set; }     // Razorpay Order ID
        public string RazorpaySignature { get; set; }   // Razorpay Signature (HMAC)
    }
}
