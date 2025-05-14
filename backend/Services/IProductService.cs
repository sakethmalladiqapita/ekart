public interface IProductService
{
    Task<List<Product>> GetAllAsync();
     Task<Product?> GetByIdAsync(string id); 
}