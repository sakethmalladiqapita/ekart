using ekart.Models;
using Events.Messages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IMongoCollection<Order> _orders;
    private readonly IMessageSession _messageSession;
    private readonly OrderFactory _orderFactory;

    public CreateOrderHandler(
        IMongoClient client,
        IOptions<DatabaseSettings> settings,
        IMessageSession messageSession,
        OrderFactory orderFactory)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
        _messageSession = messageSession;
        _orderFactory = orderFactory;
    }

    public async Task<Unit> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = _orderFactory.CreateFromCartItems(command.UserId, command.ProductItems, command.TotalAmount);
        await _orders.InsertOneAsync(order, cancellationToken: cancellationToken);

        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount
        };

        await _messageSession.Publish(orderEvent);
        return Unit.Value;
    }
}