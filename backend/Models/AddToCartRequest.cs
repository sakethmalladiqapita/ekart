namespace ekart.Models
{
    // Request to add a product to a user's cart
    public class AddToCartRequest
    {
        public string UserId { get; set; }    // User performing the action
        public string ProductId { get; set; } // Product to add
        public int Quantity { get; set; }      // Quantity to add
    }
}
