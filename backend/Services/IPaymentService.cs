using ekart.Models;

namespace ekart.Services
{
    public interface IPaymentService
    {
        // Create a new Razorpay order
        Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount);

        // Confirm payment after successful transaction
        Task ConfirmPaymentAsync(ConfirmPaymentRequest request);

        // Get status of a payment for a specific order
        Task<string> GetPaymentStatusAsync(string orderId);
    }
}
