using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;

namespace ekart.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 👤 Register a new user
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateUserAsync(user);
            return Ok(createdUser);
        }
    }
}
