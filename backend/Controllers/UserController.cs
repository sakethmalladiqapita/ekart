using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ekart.Services;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _users;
private readonly IUserService _userService;
    public UserController(IConfiguration configuration,IUserService userService)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _users = database.GetCollection<User>(configuration["MongoDB:UserCollection"]);
        _userService = userService;
    }

[HttpPost("create")]
public async Task<IActionResult> Create([FromBody] User user)
{
    try
    {
        var created = await _userService.CreateUserAsync(user);
        return Ok(created);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}


}
