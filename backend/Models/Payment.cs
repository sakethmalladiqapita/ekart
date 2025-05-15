
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("orderId")]
    public string OrderId { get; set; }

    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonElement("paymentStatus")]
    public string PaymentStatus { get; set; }

    [BsonElement("paymentDate")]
    public DateTime PaymentDate { get; set; }
    public string? Signature { get; set; }

}