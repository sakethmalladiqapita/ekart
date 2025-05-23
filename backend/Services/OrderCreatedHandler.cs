using CommandHandlers;
using Events.Messages;

public class OrderCreatedHandler : IHandleMessages<OrderCreatedEvent>
{
    private readonly CreateRazorpayOrderHandler _createOrderHandler;

    public OrderCreatedHandler(CreateRazorpayOrderHandler createOrderHandler)
    {
        _createOrderHandler = createOrderHandler;
    }

    public async Task Handle(OrderCreatedEvent message, IMessageHandlerContext context)
    {
        var command = new CreateRazorpayOrderCommand
        {
            OrderId = message.OrderId,
            Amount = message.TotalAmount
        };

        await _createOrderHandler.Handle(command, CancellationToken.None); // ✅ fix
    }
}
