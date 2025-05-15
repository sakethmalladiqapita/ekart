using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; }

    [BsonElement("address")]
    public Address? Address { get; set; }

    [BsonElement("orders")]
    public List<OrderSummary> Orders { get; set; } = new();

    [BsonElement("cart")]
    public List<CartItem> Cart { get; set; } = new();
}