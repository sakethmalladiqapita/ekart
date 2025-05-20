using ekart.Models;
using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace CommandHandlers
{
    /// <summary>
    /// CQRS Command Handler.
    /// Handles CreateRazorpayOrderCommand to initiate a Razorpay order.
    /// This bridges the internal Order aggregate with the external payment gateway.
    /// </summary>
    public class CreateRazorpayOrderHandler
    {
        private readonly string _key;
        private readonly string _secret;

        public CreateRazorpayOrderHandler(IOptions<RazorpaySettings> razorpayOptions)
        {
            _key = razorpayOptions.Value.Key;
            _secret = razorpayOptions.Value.Secret;
        }

        /// <summary>
        /// Creates a new Razorpay order using the Razorpay SDK.
        /// Amount must be in paise and currency set to INR.
        /// </summary>
        public async Task<object> Handle(CreateRazorpayOrderCommand command)
        {
            var client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>
            {
                { "amount", (int)(command.Amount * 100) }, // Razorpay expects amount in paise
                { "currency", "INR" },
                { "receipt", command.OrderId }
            };

            var order = client.Order.Create(options);
            return await Task.FromResult(order.Attributes); // Return Razorpay order metadata
        }
    }
}
