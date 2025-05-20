using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using ekart.Models;
using ekart.Services;

namespace ekart.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        // MongoDB user collection
        private readonly IMongoCollection<User> _users;

        // User service abstraction
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            // Initialize MongoDB connection and get users collection
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _users = database.GetCollection<User>(configuration["MongoDB:UserCollection"]);

            _userService = userService;
        }

        // Endpoint to create a new user (no authorization required)
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _userService.CreateUserAsync(user);
            return Ok(created);
        }
    }
}