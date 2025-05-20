// Value Object: Represents product item inside a cart
namespace ekart.Models
{
    // Represents an item in the user's cart
    public class CartItem
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; } // Display image for frontend
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
