using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;
using System.Security.Claims;

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

        // Utility to extract UserId from JWT token
        private string GetUserIdFromToken()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        // Add product to user's cart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            await _userService.AddToCartAsync(userId, request.ProductId, request.Quantity);
            return Ok();
        }
    
        // Get user's current cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserIdFromToken();
            var cart = await _userService.GetCartAsync(userId);
            return Ok(cart);
        }

        // Checkout current cart (place an order)
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutCart()
        {
            var userId = GetUserIdFromToken();
            var order = await _userService.CheckoutCartAsync(userId);
            return Ok(order);
        }

    }
}
