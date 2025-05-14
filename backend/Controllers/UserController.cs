using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _users;

    public UserController(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _users = database.GetCollection<User>(configuration["MongoDB:UserCollection"]);
    }

[HttpPost("create")]
public async Task<IActionResult> CreateUser([FromBody] User user)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var existing = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
    if (existing != null)
        return BadRequest("User already exists with this email.");

    user.Id = ObjectId.GenerateNewId().ToString(); // ✅ generate ID here
    user.Orders = new List<OrderSummary>();
    user.Cart = new List<CartItem>();

    await _users.InsertOneAsync(user);
    return Ok(user);
}

}
