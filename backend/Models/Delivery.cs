// Entity: Delivery
// - Has Id
// - Tracks status of an order delivery
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ekart.Models
{
    // Delivery tracking data for an order
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("orderId")]
        public string OrderId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("deliveryStatus")]
        public string DeliveryStatus { get; set; } // e.g., In Transit, Delivered

        [BsonElement("estimatedDeliveryDate")]
        public DateTime ExpectedDate { get; set; } // Estimated delivery date
    }
}
