using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ekart.Models;

/// <summary>
/// CQRS Command Handler.
/// Handles CreateUserCommand to register a new user into the system.
/// Validates uniqueness and required fields before insertion.
/// </summary>
public class CreateUserHandler
{
    private readonly IMongoCollection<User> _users;

    public CreateUserHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    /// <summary>
    /// Handles user creation by checking for duplicates and inserting a new user document.
    /// Throws if user already exists or input is invalid.
    /// </summary>
    public async Task<User> Handle(CreateUserCommand command)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            throw new Exception("Email and password are required");

        // Ensure email is not already registered
        var existing = await _users.Find(u => u.Email == command.Email).FirstOrDefaultAsync();
        if (existing != null)
            throw new Exception("User already exists");

        // Create new user document
        var user = new User
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Email = command.Email,
            PasswordHash = command.Password, // NOTE: Use hashed password in production
            Orders = new List<OrderSummary>(),
            Cart = new List<CartItem>()
        };

        await _users.InsertOneAsync(user);
        return user;
    }
}
