using ekart.Models;
using ekart.Services;
using Events.Messages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Command Handler.
/// Handles BuyNowCommand to immediately place an order for a single product and publish a domain event.
/// </summary>
public class BuyNowHandler
{
    private readonly IMongoCollection<Order> _orders;
    private readonly IProductService _productService;
    private readonly OrderFactory _orderFactory;
    private readonly IMessageSession _messageSession;

    public BuyNowHandler(
        IMongoClient client,
        IOptions<DatabaseSettings> settings,
        IProductService productService,
        OrderFactory orderFactory,
        IMessageSession messageSession)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>(settings.Value.OrderCollection);
        _productService = productService;
        _orderFactory = orderFactory;
        _messageSession = messageSession;
    }

    /// <summary>
    /// Handles a BuyNowCommand by validating the product, creating an order, saving it, and publishing an OrderCreatedEvent.
    /// </summary>
    public async Task<Order> Handle(BuyNowCommand command)
    {
        var product = await _productService.GetByIdAsync(command.ProductId);
        if (product == null)
            throw new Exception("Product not found");

        // Use the factory to build a valid Order aggregate
        var order = _orderFactory.CreateForDirectPurchase(product, command.Quantity, command.UserId);

        await _orders.InsertOneAsync(order);

        // Publish a domain event after successful order creation
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount
        };

        await _messageSession.Publish(orderEvent); // Asynchronously notify other services

        return order;
    }
}
