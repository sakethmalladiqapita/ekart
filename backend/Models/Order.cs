using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("productItems")]
    public List<OrderItem> ProductItems { get; set; } = new();

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("orderDate")]
    public DateTime OrderDate { get; set; }

    [BsonElement("totalAmount")]
    public decimal TotalAmount { get; set; }
}

public class OrderItem  // Value Object
{
    public string ProductId { get; set; }
     public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
}
