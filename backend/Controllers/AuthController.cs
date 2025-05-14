using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    Console.WriteLine($"Login attempt: {request.Email} / {request.Password}");

    var user = await _userService.AuthenticateAsync(request.Email, request.Password);

    if (user == null)
    {
        Console.WriteLine("Authentication failed.");
        return Unauthorized();
    }

    Console.WriteLine("Authentication successful.");
    return Ok(user);
}

}
