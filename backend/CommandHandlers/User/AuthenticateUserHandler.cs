using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;
using BCrypt.Net;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, User?>
{
    private readonly IMongoCollection<User> _users;

    public AuthenticateUserHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
    {
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _users = db.GetCollection<User>(settings.Value.UserCollection);
    }

    public async Task<User?> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _users.Find(u => u.Email == command.Email).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return null;

        bool isValid = BCrypt.Net.BCrypt.Verify(command.Password.Trim(), user.PasswordHash);
        return isValid ? user : null;
    }
}
