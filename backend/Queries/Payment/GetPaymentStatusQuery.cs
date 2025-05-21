using MediatR;

public class GetPaymentStatusQuery : IRequest<string>
{
    public string OrderId { get; set; }
}
