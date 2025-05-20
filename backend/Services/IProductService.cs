using ekart.Models;

namespace ekart.Services
{
    public interface IProductService
    {
        // Get list of all products
        Task<List<Product>> GetAllAsync();

        // Get a product by its ID
        Task<Product?> GetByIdAsync(string id);
    }
}
