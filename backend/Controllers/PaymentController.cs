using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePaymentOrder([FromBody] PaymentOrderRequest request)
    {
        var razorpayOrder = await _paymentService.CreateRazorpayOrderAsync(request.OrderId, request.Amount);
        return Ok(razorpayOrder);
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
    {
        await _paymentService.ConfirmPaymentAsync(request);
        return Ok();
    }

    [HttpGet("status/{orderId}")]
    public async Task<IActionResult> GetPaymentStatus(string orderId)
    {
        var status = await _paymentService.GetPaymentStatusAsync(orderId);
        return Ok(status);
    }
}
