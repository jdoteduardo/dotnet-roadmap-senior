using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Services;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IRepository<Product>>();
            _mapper = new Mock<IMapper>();

            _productService = new ProductService(
                _productRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop" },
                new Product { Id = 2, Name = "Smartphone" }
            };
            _productRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);
            _mapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(It.IsAny<IEnumerable<Product>>()))
                .Returns((IEnumerable<Product> src) => src.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name
                }));

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop" };
            _productRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);
            _mapper.Setup(m => m.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns((Product src) => new ProductDTO
                {
                    Id = src.Id,
                    Name = src.Name
                });

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.Name);
        }
    }
}
