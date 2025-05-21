// ✅ CreateUserHandler with email validation, password policy, and hashing
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ekart.Models;
using MediatR;
using System.Text.RegularExpressions;
using BCrypt.Net;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IMongoCollection<User> _users;

    public CreateUserHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Email and password are required.");

        // ✅ Email format validation
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        if (!emailRegex.IsMatch(command.Email))
            throw new ArgumentException("Invalid email format.");

        // ✅ Password complexity: at least 6 chars, one number, one uppercase
        if (command.Password.Length < 6 ||
            !command.Password.Any(char.IsDigit) ||
            !command.Password.Any(char.IsUpper))
        {
            throw new ArgumentException("Password must be at least 6 characters, contain a number and an uppercase letter.");

        }

        var existing = await _users.Find(u => u.Email == command.Email).FirstOrDefaultAsync(cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("User already exists.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password.Trim());

        var user = new User
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Email = command.Email,
            PasswordHash = hashedPassword,
            Orders = new List<OrderSummary>(),
            Cart = new List<CartItem>()
        };

        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }
}
