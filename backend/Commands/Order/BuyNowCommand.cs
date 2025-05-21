using MediatR;
using ekart.Models;

public class BuyNowCommand : IRequest<Order>
{
    public string UserId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
