using Razorpay.Api;
using System.Collections.Generic;

public static class RazorpayTranslator
{
    public static Dictionary<string, object> ToRazorpayOrderRequest(string orderId, decimal amount)
    {
        return new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) },
            { "currency", "INR" },
            { "receipt", orderId }
        };
    }

    public static object ToResponseAttributes(Order order)
    {
        return order.Attributes;
    }
}