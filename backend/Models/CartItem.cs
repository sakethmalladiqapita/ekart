public class CartItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; } // Add this
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
