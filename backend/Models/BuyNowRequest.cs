namespace ekart.Models
{
    // Request to place a one-click order without using the cart
    public class BuyNowRequest
    {
        public string ProductId { get; set; } // Product to purchase
        public int Quantity { get; set; }      // Quantity to purchase
    }
}
