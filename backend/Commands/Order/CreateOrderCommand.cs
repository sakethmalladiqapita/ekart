using ekart.Models;
using MediatR;

public class CreateOrderCommand : IRequest
{
    public string UserId { get; set; }
    public List<OrderItem> ProductItems { get; set; }
    public decimal TotalAmount { get; set; }
}