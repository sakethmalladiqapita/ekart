using MediatR;

public class ConfirmPaymentCommand : IRequest
{
    public string OrderId { get; set; }
}