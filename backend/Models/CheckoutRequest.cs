namespace ekart.Models
{
    // Request to trigger checkout of cart items
    public class CheckoutCartRequest
    {
        public string UserId { get; set; } // User initiating the checkout
    }
}
