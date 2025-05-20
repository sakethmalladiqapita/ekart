using ekart.Models;

namespace ekart.Services
{
    public interface IUserService
    {
        // Authenticate user based on email and password
        Task<User?> AuthenticateAsync(string email, string password);

        // Add a product to user's cart
        Task AddToCartAsync(string userId, string productId, int quantity);

        // Retrieve all cart items for a user
        Task<List<CartItem>> GetCartAsync(string userId);

        // Checkout the cart and place an order
        Task<Order> CheckoutCartAsync(string userId);

        // Create a new user
        Task<User> CreateUserAsync(User user);
    }
}
