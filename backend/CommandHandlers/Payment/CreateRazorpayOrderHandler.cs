using ekart.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace CommandHandlers
{
    public class CreateRazorpayOrderHandler : IRequestHandler<CreateRazorpayOrderCommand, object>
    {
        private readonly string _key;
        private readonly string _secret;

        public CreateRazorpayOrderHandler(IOptions<RazorpaySettings> razorpayOptions)
        {
            _key = razorpayOptions.Value.Key;
            _secret = razorpayOptions.Value.Secret;
        }

    public async Task<object> Handle(CreateRazorpayOrderCommand command, CancellationToken cancellationToken)
    {
        var client = new RazorpayClient(_key, _secret);
        var options = RazorpayTranslator.ToRazorpayOrderRequest(command.OrderId, command.Amount);
        var order = client.Order.Create(options);
        return await Task.FromResult(RazorpayTranslator.ToResponseAttributes(order));
    }

    }
}