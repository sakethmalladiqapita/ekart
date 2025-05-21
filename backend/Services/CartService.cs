using MediatR;
using ekart.Models;

namespace ekart.Services
{
    public class CartService : ICartService
    {
        private readonly IMediator _mediator;

        public CartService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddToCartAsync(string userId, string productId, int quantity)
        {
            await _mediator.Send(new AddToCartCommand
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _mediator.Send(new GetCartQuery { UserId = userId });
        }

        public async Task<Order?> CheckoutCartAsync(string userId)
        {
            return await _mediator.Send(new CheckoutCartCommand { UserId = userId });
        }
    }
}
