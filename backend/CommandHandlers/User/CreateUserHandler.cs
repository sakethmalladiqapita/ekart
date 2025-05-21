using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ekart.Models;
using MediatR;

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
            throw new Exception("Email and password are required");

        var existing = await _users.Find(u => u.Email == command.Email).FirstOrDefaultAsync(cancellationToken);
        if (existing != null)
            throw new Exception("User already exists");

        var user = new User
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Email = command.Email,
            PasswordHash = command.Password,
            Orders = new List<OrderSummary>(),
            Cart = new List<CartItem>()
        };

        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return user;
    }
}
