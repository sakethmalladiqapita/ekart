using MediatR;

public class CreateRazorpayOrderCommand : IRequest<object>
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
}