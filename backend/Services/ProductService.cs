using ekart.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ekart.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>(settings.Value.ProductCollection);
        }

        // Get all products
        public async Task<List<Product>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        // Get a product by ID
        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // Get paginated product list
        public async Task<List<Product>> GetProductsAsync(int page = 1, int pageSize = 8)
        {
            return await _products.Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        // Total count of products for pagination
        public async Task<int> GetTotalProductCountAsync()
        {
            return (int)await _products.CountDocumentsAsync(_ => true);
        }
    }
}
