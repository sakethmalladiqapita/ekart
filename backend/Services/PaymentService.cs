using CommandHandlers;
using ekart.Models;

namespace ekart.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly CreateRazorpayOrderHandler _createOrderHandler;
        private readonly ConfirmPaymentHandler _confirmHandler;
        private readonly GetPaymentStatusHandler _statusHandler;

        public PaymentService(
            CreateRazorpayOrderHandler createOrderHandler,
            ConfirmPaymentHandler confirmHandler,
            GetPaymentStatusHandler statusHandler)
        {
            _createOrderHandler = createOrderHandler;
            _confirmHandler = confirmHandler;
            _statusHandler = statusHandler;
        }

        // Create Razorpay order
        public async Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount)
        {
            return await _createOrderHandler.Handle(new CreateRazorpayOrderCommand
            {
                OrderId = orderId,
                Amount = amount
            });
        }

        // Confirm payment
        public async Task ConfirmPaymentAsync(ConfirmPaymentRequest request)
        {
            await _confirmHandler.Handle(new ConfirmPaymentCommand { OrderId = request.OrderId });
        }

        // Get payment status
        public async Task<string> GetPaymentStatusAsync(string orderId)
        {
            return await _statusHandler.Handle(new GetPaymentStatusQuery { OrderId = orderId });
        }
    }
}
