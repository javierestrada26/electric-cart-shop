using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;
using ElectricCartShop.API.Services;

namespace ElectricCartShop.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IJsonDatabaseService _databaseService;

        public ProductRepository(IJsonDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var data = await _databaseService.LoadDataAsync();
            return data.Products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.Products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var data = await _databaseService.LoadDataAsync();
            
            product.Id = data.Counters.ProductId;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            
            data.Products.Add(product);
            data.Counters.ProductId++;
            
            await _databaseService.SaveDataAsync(data);
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var data = await _databaseService.LoadDataAsync();
            var existingProduct = data.Products.FirstOrDefault(p => p.Id == product.Id);
            
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Image = product.Image;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Sizes = product.Sizes;
                existingProduct.UpdatedAt = DateTime.UtcNow;
                
                await _databaseService.SaveDataAsync(data);
            }
            
            return existingProduct!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            var product = data.Products.FirstOrDefault(p => p.Id == id);
            
            if (product != null)
            {
                data.Products.Remove(product);
                await _databaseService.SaveDataAsync(data);
                return true;
            }
            
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var data = await _databaseService.LoadDataAsync();
            return data.Products.Any(p => p.Id == id);
        }
    }
}


