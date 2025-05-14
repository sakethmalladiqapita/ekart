using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Delivery
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("orderId")]
    public string OrderId { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("deliveryStatus")]
    public string DeliveryStatus { get; set; }

    [BsonElement("estimatedDeliveryDate")]
    public DateTime ExpectedDate { get; set; }
}
