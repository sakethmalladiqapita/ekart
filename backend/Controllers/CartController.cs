using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly IUserService _userService;

    public CartController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        await _userService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        var cart = await _userService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutCart([FromBody] CheckoutCartRequest request)
    {
        var order = await _userService.CheckoutCartAsync(request.UserId);
        return Ok(order);
    }
}
