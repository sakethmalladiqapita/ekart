using MediatR;
using ekart.Models;

public class CheckoutCartCommand : IRequest<Order?>
{
    public string UserId { get; set; }
}