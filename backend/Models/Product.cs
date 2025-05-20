// Entity: Product
// - Has Id, persists in database
// - Belongs to catalog domain
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ekart.Models
{
    // Represents a product in the catalog
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } // Product name

        [BsonElement("price")]
        public decimal Price { get; set; } // Selling price

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } // Image URL for product display

        [BsonElement("stock")]
        public int Stock { get; set; } // Units available in inventory

        [BsonElement("category")]
        public ProductCategory Category { get; set; } // Type/category of product
    }

    // Enum to represent product categories
    public enum ProductCategory
    {
        Electronics,
        Apparel,
        Books
    }
}
