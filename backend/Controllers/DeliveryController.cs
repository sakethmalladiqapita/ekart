using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/delivery")]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet("status/{orderId}")]
    public async Task<IActionResult> GetStatus(string orderId)
    {
        var status = await _deliveryService.GetStatusAsync(orderId);
        return Ok(status);
    }
    
}
