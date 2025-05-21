using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Services;

namespace ekart.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/delivery")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        // ✅ Get delivery status by order ID
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetStatus(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return BadRequest("Invalid Order ID");

            var status = await _deliveryService.GetStatusAsync(orderId);
            if (string.IsNullOrWhiteSpace(status))
                return NotFound(new { message = "No delivery info found for this order." });

            return Ok(new { orderId, deliveryStatus = status });
        }
    }
}
