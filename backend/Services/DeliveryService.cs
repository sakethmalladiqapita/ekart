namespace ekart.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly GetDeliveryStatusHandler _statusHandler;

        public DeliveryService(GetDeliveryStatusHandler statusHandler)
        {
            _statusHandler = statusHandler;
        }

        // Fetch current delivery status for the given order ID using CQRS query handler
        public async Task<string> GetStatusAsync(string orderId)
        {
            return await _statusHandler.Handle(new GetDeliveryStatusQuery
            {
                OrderId = orderId
            });
        }
    }
}
