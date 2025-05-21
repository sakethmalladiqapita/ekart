using MediatR;
using ekart.Models;

public class GetUserOrdersQuery : IRequest<List<Order>>
{
    public string UserId { get; set; }
}

