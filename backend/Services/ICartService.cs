namespace ekart.Services
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, string productId, int quantity);
        Task<List<CartItem>> GetCartAsync(string userId);
        Task<Order> CheckoutCartAsync(string userId);
    }
}
