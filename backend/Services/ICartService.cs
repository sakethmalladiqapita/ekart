using ekart.Models;

namespace ekart.Services
{
    public interface ICartService
    {
        // Add product to cart
        Task AddToCartAsync(string userId, string productId, int quantity);

        // Get cart items for a user
        Task<List<CartItem>> GetCartAsync(string userId);

        // Checkout and return order
        Task<Order> CheckoutCartAsync(string userId);
    }
}
