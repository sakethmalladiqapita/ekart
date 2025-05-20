// Aggregate Root Entity: User
// - Entity (has Id)
// - Owns Address (VO), CartItem (VO), OrderSummary (summary VO)

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ekart.Models
{
    // Represents a registered user of the platform
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // MongoDB unique identifier

        [BsonElement("email")]
        public string Email { get; set; } // User's email for login

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } // Hashed password

        [BsonElement("address")]
        public Address Address { get; set; } // User's shipping address

        [BsonElement("orders")]
        public List<OrderSummary> Orders { get; set; } = new(); // List of past order summaries

        [BsonElement("cart")]
        public List<CartItem> Cart { get; set; } = new(); // User's current shopping cart
    }
}
