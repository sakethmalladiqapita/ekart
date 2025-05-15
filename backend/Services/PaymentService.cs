using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Razorpay.Api;

public class PaymentService : IPaymentService
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly IMongoCollection<Order> _orders;
    private readonly IMongoCollection<Delivery> _deliveries;
    private readonly string _razorpayKey;
    private readonly string _razorpaySecret;


      public PaymentService(
        IOptions<DatabaseSettings> dbSettings,
        IOptions<RazorpaySettings> razorpayOptions,
        IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = database.GetCollection<Payment>(dbSettings.Value.PaymentCollection);
        _orders = database.GetCollection<Order>(dbSettings.Value.OrderCollection);
        _deliveries = database.GetCollection<Delivery>("DeliveriesData");

        _razorpayKey = razorpayOptions.Value.Key;
        _razorpaySecret = razorpayOptions.Value.Secret;
    }

    public async Task<object> CreateRazorpayOrderAsync(string orderId, decimal amount)
    {
        var options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) },  // Razorpay expects paise
            { "currency", "INR" },
            { "receipt", orderId }
        };

        var client = new RazorpayClient(_razorpayKey, _razorpaySecret);
        var order = client.Order.Create(options);
        return order.Attributes;
    }

        public async Task ConfirmPaymentAsync(ConfirmPaymentRequest request)
    {
        var payment = new Payment
        {
            Id = ObjectId.GenerateNewId().ToString(),
            OrderId = request.OrderId,
            Amount = 0,
            PaymentStatus = "Success",
            PaymentDate = DateTime.UtcNow
        };

        await _payments.InsertOneAsync(payment);

        var updateOrder = Builders<Order>.Update.Set(o => o.Status, "Successful");
        await _orders.UpdateOneAsync(o => o.Id == request.OrderId, updateOrder);

        var delivery = new Delivery
        {
            OrderId = request.OrderId,
            DeliveryStatus = "Processing",
            ExpectedDate = DateTime.UtcNow.AddDays(3)
        };
        await _deliveries.InsertOneAsync(delivery);
    }


    public async Task<string> GetPaymentStatusAsync(string orderId)
    {
        var payment = await _payments.Find(p => p.OrderId == orderId).FirstOrDefaultAsync();
        return payment?.PaymentStatus ?? "Not Paid";
    }
}