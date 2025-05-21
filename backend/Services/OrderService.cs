// ✅ Refactored OrderService.cs using MediatR and IHttpContextAccessor
using System.Security.Claims;
using ekart.Models;
using MediatR;

namespace ekart.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderService(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        private string GetUserId()
        {
            return _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _mediator.Send(new CreateOrderCommand
            {
                UserId = GetUserId(),
                ProductItems = order.ProductItems,
                TotalAmount = order.TotalAmount
            });
        }

        public async Task<List<Order>> GetUserOrdersAsync()
        {
            return await _mediator.Send(new GetUserOrdersQuery
            {
                UserId = GetUserId()
            });
        }

        public async Task<Order> BuyNowAsync(string productId, int quantity)
        {
            return await _mediator.Send(new BuyNowCommand
            {
                UserId = GetUserId(),
                ProductId = productId,
                Quantity = quantity
            });
        }
    }
}
