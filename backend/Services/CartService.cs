using ekart.Models;

namespace ekart.Services
{
    public class CartService : ICartService
    {
        private readonly AddToCartHandler _addToCartHandler;
        private readonly GetCartHandler _getCartHandler;
        private readonly CheckoutCartHandler _checkoutCartHandler;

        public CartService(
            AddToCartHandler addToCartHandler,
            GetCartHandler getCartHandler,
            CheckoutCartHandler checkoutCartHandler)
        {
            _addToCartHandler = addToCartHandler;
            _getCartHandler = getCartHandler;
            _checkoutCartHandler = checkoutCartHandler;
        }

        // Add product to cart
        public async Task AddToCartAsync(string userId, string productId, int quantity)
        {
            var command = new AddToCartCommand
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            await _addToCartHandler.Handle(command);
        }

        // Get user's cart
        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _getCartHandler.Handle(new GetCartQuery { UserId = userId });
        }

        // Checkout cart
        public async Task<Order?> CheckoutCartAsync(string userId)
        {
            return await _checkoutCartHandler.Handle(new CheckoutCartCommand { UserId = userId });
        }
    }
}
