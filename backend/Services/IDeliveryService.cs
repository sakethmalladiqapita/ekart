public interface IDeliveryService
{
    Task<string> GetStatusAsync(string orderId);
}
