using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles AuthenticateUserCommand to validate user credentials against the database.
/// </summary>
public class AuthenticateUserHandler
{
    private readonly IMongoCollection<User> _users;

    public AuthenticateUserHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    /// <summary>
    /// Attempts to authenticate a user by comparing the email and password.
    /// NOTE: This example uses plaintext passwords – in production, always hash and salt passwords.
    /// </summary>
    public async Task<User?> Handle(AuthenticateUserCommand command)
    {
        var user = await _users.Find(u => u.Email == command.Email).FirstOrDefaultAsync();

        // Basic comparison — replace with secure hash comparison in production
        return user?.PasswordHash.Trim() == command.Password.Trim() ? user : null;
    }
}
