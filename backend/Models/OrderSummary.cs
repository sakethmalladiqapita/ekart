// Value Object: Order summary snapshot for User entity
namespace ekart.Models
{
    // A brief summary of an order, stored in the user's profile
    public class OrderSummary
    {
        public string? OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Order status
        public DateTime OrderDate { get; set; }
    }
}
