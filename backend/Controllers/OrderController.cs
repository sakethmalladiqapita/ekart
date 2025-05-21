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

        // ✅ Place a one-click order ("Buy Now" flow)
        [HttpPost("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] BuyNowRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.BuyNowAsync(request.ProductId, request.Quantity);
            return Ok(order);
        }

        // ✅ Get orders of the current logged-in user
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var orders = await _orderService.GetUserOrdersAsync();
            return Ok(orders);
        }
    }
}
