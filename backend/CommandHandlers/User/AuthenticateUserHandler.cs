using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;

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
        return user?.PasswordHash.Trim() == command.Password.Trim() ? user : null;
    }
}
