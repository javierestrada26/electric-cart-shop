using ElectricCartShop.API.DTOs;
using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;

namespace ElectricCartShop.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToDto(product) : null;
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Image = createProductDto.Image,
                Description = createProductDto.Description,
                Category = createProductDto.Category,
                Sizes = createProductDto.Sizes
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToDto(createdProduct);
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new ArgumentException($"Product with ID {id} not found.");

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Image = updateProductDto.Image;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Category = updateProductDto.Category;
            existingProduct.Sizes = updateProductDto.Sizes;

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            return MapToDto(updatedProduct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
                Category = product.Category,
                Sizes = product.Sizes
            };
        }
    }
}


