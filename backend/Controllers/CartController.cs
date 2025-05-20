using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;

namespace ekart.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IUserService _userService;

        public CartController(IUserService userService)
        {
            _userService = userService;
        }

        // Add product to user's cart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
            return Ok();
        }

        // Get user's current cart
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Invalid User ID");

            var cart = await _userService.GetCartAsync(userId);
            return Ok(cart);
        }

        // Checkout current cart (place an order)
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCart([FromBody] CheckoutCartRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _userService.CheckoutCartAsync(request.UserId);
            return Ok(order);
        }
    }
}