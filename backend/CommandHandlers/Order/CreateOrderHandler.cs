using ekart.Models;
using Events.Messages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles CreateOrderCommand to place an order (usually from cart checkout) and publish an OrderCreatedEvent.
/// </summary>
public class CreateOrderHandler
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

    /// <summary>
    /// Creates a new order from provided command data and publishes a domain event.
    /// </summary>
    public async Task Handle(CreateOrderCommand command)
    {
        // Build order using a centralized factory for consistency and reuse
        var order = _orderFactory.CreateFromCartItems(
            command.UserId,
            command.ProductItems,
            command.TotalAmount
        );

        await _orders.InsertOneAsync(order);

        // Emit domain event for side effects (e.g., payment creation)
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount
        };

        await _messageSession.Publish(orderEvent);
    }
}
