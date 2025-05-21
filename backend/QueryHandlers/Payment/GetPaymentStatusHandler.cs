using MediatR;
using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class GetPaymentStatusHandler : IRequestHandler<GetPaymentStatusQuery, string>
{
    private readonly IMongoCollection<Payment> _payments;

    public GetPaymentStatusHandler(IMongoClient client, IOptions<DatabaseSettings> dbSettings)
    {
        var db = client.GetDatabase(dbSettings.Value.DatabaseName);
        _payments = db.GetCollection<Payment>(dbSettings.Value.PaymentCollection);
    }

    public async Task<string> Handle(GetPaymentStatusQuery query, CancellationToken cancellationToken)
    {
        var payment = await _payments.Find(p => p.OrderId == query.OrderId).FirstOrDefaultAsync(cancellationToken);
        return payment?.PaymentStatus ?? "Not Paid";
    }
}
