using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;

namespace ekart.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Create a Razorpay payment order
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] PaymentOrderRequest request)
        {
            if (!ModelState.IsValid || request.Amount <= 0)
                return BadRequest("Invalid payment details.");

            var order = await _paymentService.CreateRazorpayOrderAsync(request.OrderId, request.Amount);
            return Ok(order);
        }

        // Confirm the Razorpay payment
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _paymentService.ConfirmPaymentAsync(request);
            return Ok();
        }

        // Get payment status for a given order ID
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId)) return BadRequest("Invalid Order ID");

            var status = await _paymentService.GetPaymentStatusAsync(orderId);
            return Ok(status);
        }
    }
}