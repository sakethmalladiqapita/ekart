using MediatR;

public class AddToCartCommand : IRequest<Unit>
{
    public string UserId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}