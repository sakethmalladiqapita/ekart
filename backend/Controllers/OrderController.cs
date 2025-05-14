using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("buy-now")]
    public async Task<IActionResult> BuyNow([FromBody] BuyNowRequest request)
    {
        var order = await _orderService.BuyNowAsync(request.UserId, request.ProductId, request.Quantity);
        return Ok(order);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserOrders(string userId)
    {
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }
}
