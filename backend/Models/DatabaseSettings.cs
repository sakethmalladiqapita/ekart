namespace ekart.Models
{
    // App-level database settings loaded from configuration
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollection { get; set; } = null!;
        public string ProductCollection { get; set; } = null!;
        public string OrderCollection { get; set; } = null!;
        public string PaymentCollection { get; set; } = null!;
        public string DeliveryCollection { get; set; } = null!;
    }
}
