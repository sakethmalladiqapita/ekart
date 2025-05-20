using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;

namespace ekart.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Place a one-click order ("Buy Now" flow)
        [HttpPost("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] BuyNowRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.BuyNowAsync(request.UserId, request.ProductId, request.Quantity);
            return Ok(order);
        }

        // Get all orders for a specific user
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Invalid User ID");

            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
    }
}
