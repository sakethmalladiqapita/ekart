using MediatR;

namespace ekart.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IMediator _mediator;

        public DeliveryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GetStatusAsync(string orderId)
        {
            return await _mediator.Send(new GetDeliveryStatusQuery
            {
                OrderId = orderId
            });
        }
    }
}
