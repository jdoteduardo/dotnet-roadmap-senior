using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO?>(product);
        }

        public async Task<ProductDTO> CreateProductAsync(string name, int categoryId, decimal price)
        {
            var product = new Product 
            { 
                Name = name, 
                CategoryId = categoryId, 
                Price = price,
                CreatedAt = DateTime.UtcNow
            };
            var createdProduct = await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDTO>(createdProduct);
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, string name, int categoryId, decimal price)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new Exception($"Product {id} not found");

            var product = new Product 
            { 
                Id = id, 
                Name = name, 
                CategoryId = categoryId, 
                Price = price,
                CreatedAt = existingProduct.CreatedAt
            };
            var updatedProduct = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductDTO>(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}