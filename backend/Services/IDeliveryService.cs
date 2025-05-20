namespace ekart.Services
{
    public interface IDeliveryService
    {
        // Get delivery status of a specific order
        Task<string> GetStatusAsync(string orderId);
    }
}
