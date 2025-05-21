using ekart.Models;
using ekart.Services;
using Events.Messages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MediatR;

public class BuyNowHandler : IRequestHandler<BuyNowCommand, Order>
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

    public async Task<Order> Handle(BuyNowCommand command, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(command.ProductId);
        if (product == null)
            throw new Exception("Product not found");

        var order = _orderFactory.CreateForDirectPurchase(product, command.Quantity, command.UserId);
        await _orders.InsertOneAsync(order, cancellationToken: cancellationToken);

        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount
        };

        await _messageSession.Publish(orderEvent);
        return order;
    }
}
