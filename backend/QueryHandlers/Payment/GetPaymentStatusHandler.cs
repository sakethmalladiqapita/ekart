using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/// <summary>
/// CQRS Query Handler.
/// Handles GetPaymentStatusQuery by retrieving the payment status of a specific order.
/// This is a read-only query operation with no side effects.
/// </summary>
public class GetPaymentStatusHandler
{
    private readonly IMongoCollection<Payment> _payments;

    /// <summary>
    /// Initializes the MongoDB Payments collection reference.
    /// </summary>
    public GetPaymentStatusHandler(IMongoClient client, IOptions<DatabaseSettings> dbSettings)
    {
        var db = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = db.GetCollection<Payment>(dbSettings.Value.PaymentCollection);
    }

    /// <summary>
    /// Retrieves the payment status for a given order ID.
    /// Returns "Not Paid" if no payment record is found.
    /// </summary>
    /// <param name="query">Query containing the OrderId</param>
    /// <returns>The current payment status as a string</returns>
    public async Task<string> Handle(GetPaymentStatusQuery query)
    {
        var payment = await _payments.Find(p => p.OrderId == query.OrderId).FirstOrDefaultAsync();
        return payment?.PaymentStatus ?? "Not Paid";
    }
}
