public class OrderFactory
{
    public Order CreateFromCart(User user)
    {
        var order = new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = user.Id,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ProductItems = user.Cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                PriceAtPurchase = c.UnitPrice
            }).ToList(),
            TotalAmount = user.Cart.Sum(c => c.Quantity * c.UnitPrice)
        };

        return order;
    }

    public Order CreateForDirectPurchase(Product product, int quantity, string userId)
    {
        return new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ProductItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    PriceAtPurchase = product.Price
                }
            },
            TotalAmount = product.Price * quantity
        };
    }
}
