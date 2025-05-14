public interface IPaymentService
{
    Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount);
    Task ConfirmPaymentAsync(ConfirmPaymentRequest request);
    Task<string> GetPaymentStatusAsync(string orderId);
}