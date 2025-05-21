using ekart.Models;
using MediatR;

namespace ekart.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMediator _mediator;

        public PaymentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount)
        {
            return await _mediator.Send(new CreateRazorpayOrderCommand
            {
                OrderId = orderId,
                Amount = amount
            });
        }

        public async Task ConfirmPaymentAsync(ConfirmPaymentRequest request)
        {
            await _mediator.Send(new ConfirmPaymentCommand
            {
                OrderId = request.OrderId
            });
        }

        public async Task<string> GetPaymentStatusAsync(string orderId)
        {
            return await _mediator.Send(new GetPaymentStatusQuery
            {
                OrderId = orderId
            });
        }
    }
}