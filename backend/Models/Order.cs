// Aggregate Root Entity: Order
// - Entity (has Id)
// - Owns OrderItem (Value Object)
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ekart.Models
{
    // Represents a customer's order containing multiple products
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } // ID of the user who placed the order

        [BsonElement("productItems")]
        public List<OrderItem> ProductItems { get; set; } = new(); // Products in the order

        [BsonElement("status")]
        public string Status { get; set; } // e.g., Pending, Shipped, Delivered

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; } // Final price after all items
    }

    // Value Object: Represents a product line in the order
    public class OrderItem
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } // Unit price when ordered
    }
}
