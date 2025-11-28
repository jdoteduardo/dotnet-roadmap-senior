using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using System.Linq;

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

        public async Task<PagedResult<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var all = (await _productRepository.GetAllAsync()).ToList();
            var total = all.Count;
            var items = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<ProductDTO>(p))
                .ToList();

            return new PagedResult<ProductDTO>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var p = await _productRepository.GetByIdAsync(id);
            return p == null ? null : _mapper.Map<ProductDTO>(p);
        }

        public async Task<ProductDTO> CreateProductAsync(string name, int categoryId, decimal price)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, string name, int categoryId, decimal price)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}