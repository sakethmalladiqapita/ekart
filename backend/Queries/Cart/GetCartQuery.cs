using ekart.Models;
using MediatR;

public class GetCartQuery : IRequest<List<CartItem>>
{
    public string UserId { get; set; }
}