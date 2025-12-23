using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Services;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;
using ECommerce.OrderManagement.Domain.ValueObjects;
using ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly Mock<IRepository<Coupon>> _couponRepository;
        private readonly Mock<IRepository<Order>> _orderRepository;
        private readonly Mock<IEntityFactory<Order>> _orderFactory;
        private readonly Mock<IMapper> _mapper;
        private readonly List<IDiscountStrategy> _discountStrategies;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _productRepository = new Mock<IRepository<Product>>();
            _couponRepository = new Mock<IRepository<Coupon>>();
            _orderRepository = new Mock<IRepository<Order>>();
            _orderFactory = new Mock<IEntityFactory<Order>>();
            _mapper = new Mock<IMapper>();
            _discountStrategies = new List<IDiscountStrategy>
            {
                new FixedDiscountStrategy(),
                new PercentageDiscountStrategy()
            };

            _orderService = new OrderService(
                _orderFactory.Object,
                _productRepository.Object,
                _couponRepository.Object,
                _orderRepository.Object,
                _discountStrategies,
                _mapper.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_WithValidData_ShouldCreateOrder()
        {
            // Arrange
            var createOrderDto = new CreateOrderDTO
            {
                Items = new List<OrderItemDTO>
                {
                    new() { ProductId = 1, Quantity = 2 }
                }
            };

            var product = new Product { Id = 1, Price = 50.00m, Name = "Test Product" };
            var order = new Order
            {
                Id = 0,
                SubTotal = new Money(0m),
                OrderItems = new List<OrderItem>()
            };
            var expectedOrderDto = new OrderDTO { Id = 1, SubTotal = 100.00m };

            _orderFactory.Setup(f => f.Create()).Returns(order);
            _productRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _orderRepository.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDTO>(It.IsAny<Order>())).Returns(expectedOrderDto);

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.00m, result.SubTotal);
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderById_WithExistingOrder_ShouldReturnOrderDTO()
        {
            // Arrange
            var order = new Order { Id = 1, SubTotal = new Money(100m) };
            var orderDto = new OrderDTO { Id = 1, SubTotal = 100m };

            _orderRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDTO?>(order)).Returns(orderDto);

            // Act
            var result = await _orderService.GetOrderById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(100m, result.SubTotal);
        }

        [Fact]
        public async Task GetOrderById_WithNonExistingOrder_ShouldReturnNull()
        {
            // Arrange
            _orderRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Order?)null);
            _mapper.Setup(m => m.Map<OrderDTO?>(It.IsAny<Order?>())).Returns((OrderDTO?)null);

            // Act
            var result = await _orderService.GetOrderById(999);

            // Assert
            Assert.Null(result);
        }
    }
}
