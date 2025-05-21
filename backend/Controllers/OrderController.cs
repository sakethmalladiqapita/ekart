using ekart.Models;
using ekart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
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

        // ✅ Place a one-click order ("Buy Now" flow) with input validation
        [HttpPost("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] BuyNowRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.ProductId) || request.Quantity < 1)
                return BadRequest("Invalid product ID or quantity.");

            var order = await _orderService.BuyNowAsync(request.ProductId, request.Quantity);
            return Ok(order);
        }

        // ✅ Get paginated orders of the current logged-in user
        [HttpGet]
        public async Task<IActionResult> GetUserOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid pagination parameters.");

            var allOrders = await _orderService.GetUserOrdersAsync();
            var pagedOrders = allOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(pagedOrders);
        }
    }
}
