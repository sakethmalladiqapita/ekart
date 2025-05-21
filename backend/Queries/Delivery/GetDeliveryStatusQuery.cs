using MediatR;

public class GetDeliveryStatusQuery : IRequest<string>
{
    public string OrderId { get; set; }
}
