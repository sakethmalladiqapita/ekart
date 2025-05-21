using ekart.Models;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ekart.Handlers
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, List<CartItem>>
    {
        private readonly IMongoCollection<User> _users;

        public GetCartHandler(IMongoClient client, IOptions<DatabaseSettings> settings)
        {
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _users = db.GetCollection<User>(settings.Value.UserCollection);
        }

        public async Task<List<CartItem>> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _users.Find(u => u.Id == query.UserId).FirstOrDefaultAsync(cancellationToken);
                return user?.Cart?.Where(c => c.Quantity > 0).ToList() ?? new List<CartItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetCart] ERROR: {ex.Message}");
                throw;
            }
        }

    }
}
