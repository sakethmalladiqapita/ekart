using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Razorpay.Api;

public class PaymentService : IPaymentService
{
    private readonly IMongoCollection<Payment> _payments;
    private readonly string _razorpayKey;
    private readonly string _razorpaySecret;


    public PaymentService(IOptions<DatabaseSettings> dbSettings, IOptions<RazorpaySettings> razorpayOptions)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = database.GetCollection<Payment>(dbSettings.Value.PaymentCollection);

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
    // 1. Record payment
    var payment = new Payment
    {
        Id = ObjectId.GenerateNewId().ToString(),
        UserId = "", // Optional: Load from order if needed
        OrderId = request.OrderId,
        Amount = 0,  // Optional: can be set from Razorpay API if desired
        PaymentStatus = "Success",
        PaymentDate = DateTime.UtcNow
    };
    await _payments.InsertOneAsync(payment);

    // 2. Connect to Mongo and collections
    var client = new MongoClient("mongodb://localhost:27017"); // Or inject this
    var database = client.GetDatabase("ekartdatabase");
    var orders = database.GetCollection<Order>("OrdersData");
    var deliveries = database.GetCollection<Delivery>("DeliveriesData");

    // 3. Update order status to "Successful"
    var updateOrder = Builders<Order>.Update.Set(o => o.Status, "Successful");
    var result = await orders.UpdateOneAsync(o => o.Id == request.OrderId, updateOrder);

    // 4. Create delivery record if order update succeeded
    if (result.ModifiedCount > 0)
    {
        var delivery = new Delivery
        {
            OrderId = request.OrderId,
            DeliveryStatus = "Processing",
            ExpectedDate = DateTime.UtcNow.AddDays(3)
        };
        await deliveries.InsertOneAsync(delivery);
    }
}


    public async Task<string> GetPaymentStatusAsync(string orderId)
    {
        var payment = await _payments.Find(p => p.OrderId == orderId).FirstOrDefaultAsync();
        return payment?.PaymentStatus ?? "Not Paid";
    }
}