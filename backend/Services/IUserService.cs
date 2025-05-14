public interface IUserService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task AddToCartAsync(string userId, string productId, int quantity);
    Task<List<CartItem>> GetCartAsync(string userId);
    Task<Order> CheckoutCartAsync(string userId);
}
