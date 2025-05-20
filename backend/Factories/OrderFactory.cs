using ekart.Models;
using MongoDB.Bson;

// <summary>
// DDD Factory class.
// Responsible for creating valid Order aggregates from cart items or direct purchase.
// Encapsulates construction logic and enforces consistency at creation time.
// </summary>
public class OrderFactory
{
    // <summary>
    // Creates a new Order from a list of cart items.
    // Used during cart checkout scenarios.
    // </summary>
    // <param name="userId">ID of the user placing the order</param>
    // <param name="items">Cart items converted to order items</param>
    // <param name="totalAmount">Total price of the order</param>
    // <returns>Newly constructed Order aggregate</returns>
    public Order CreateFromCartItems(string userId, List<OrderItem> items, decimal totalAmount)
    {
        return new Order
        {
            Id = ObjectId.GenerateNewId().ToString(),
            UserId = userId,
            ProductItems = items,
            TotalAmount = totalAmount,
            Status = "Pending",
            OrderDate = DateTime.UtcNow
        };
    }

    // <summary>
    // Creates an Order for a one-click "Buy Now" purchase.
    // This is a shortcut scenario that skips the cart.
    // </summary>
    // <param name="product">Product being purchased</param>
    // <param name="quantity">Quantity of the product</param>
    // <param name="userId">User who is buying</param>
    // <returns>Order object for direct checkout</returns>
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
