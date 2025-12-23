using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Enums;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;
using ECommerce.OrderManagement.Domain.ValueObjects;
using ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies;
using FluentValidation;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Features.Handlers
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IEntityFactory<Order>> _orderFactory;
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly Mock<IRepository<Coupon>> _couponRepository;
        private readonly Mock<IRepository<Order>> _orderRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IValidator<CreateOrderCommand>> _validator;
        private readonly Mock<IMediator> _mediator;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _orderFactory = new Mock<IEntityFactory<Order>>();
            _productRepository = new Mock<IRepository<Product>>();
            _couponRepository = new Mock<IRepository<Coupon>>();
            _orderRepository = new Mock<IRepository<Order>>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<CreateOrderCommand>>();
            _mediator = new Mock<IMediator>();

            var discountStrategies = new List<IDiscountStrategy> 
            { 
                new FixedDiscountStrategy(),
                new PercentageDiscountStrategy()
            };

            _handler = new CreateOrderCommandHandler(
                _orderFactory.Object,
                _productRepository.Object,
                _couponRepository.Object,
                _orderRepository.Object,
                discountStrategies,
                _mapper.Object,
                _validator.Object,
                _mediator.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateOrder()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Items = new List<OrderItemCommand> { new() { ProductId = 1, Quantity = 2 } },
                CustomerId = 1,
                AddressId = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var product = new Product { Id = 1, Price = 50.00m, Name = "Test Product" };
            var order = new Order
            {
                Id = 1,
                SubTotal = new Money(100m),
                OrderItems = new List<OrderItem>(),
                CustomerId = 1,
                AddressId = 1
            };
            var expectedOrderDto = new OrderDTO { Id = 1, SubTotal = 100m, CustomerId = 1, AddressId = 1 };

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

            _orderFactory.Setup(f => f.Create()).Returns(order);
            _productRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _orderRepository.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDTO>(It.IsAny<Order>())).Returns(expectedOrderDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(100m, result.SubTotal);

            _orderFactory.Verify(f => f.Create(), Times.Once);
            _productRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _mapper.Verify(m => m.Map<OrderDTO>(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateOrderCommand();
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Items", "Items obrigatório"));

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithFixedCoupon_ShouldApplyFixedDiscount()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Items = new List<OrderItemCommand> { new() { ProductId = 1, Quantity = 2 } },
                CouponId = 1,
                CustomerId = 1,
                AddressId = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var product = new Product { Id = 1, Price = 50.00m, Name = "Test Product" };
            var coupon = new Coupon 
            { 
                Id = 1, 
                DiscountAmount = 10.00m,
                DiscountType = DiscountType.Fixed
            };
            var order = new Order
            {
                Id = 1,
                SubTotal = new Money(100m),
                OrderItems = new List<OrderItem>(),
                CustomerId = 1,
                AddressId = 1
            };
            var expectedOrderDto = new OrderDTO { Id = 1, SubTotal = 90m, CouponId = 1 };

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

            _orderFactory.Setup(f => f.Create()).Returns(order);
            _productRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _couponRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(coupon);
            _orderRepository.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDTO>(It.IsAny<Order>())).Returns(expectedOrderDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(90m, result.SubTotal);
            Assert.Equal(1, result.CouponId);

            _couponRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithPercentageCoupon_ShouldApplyPercentageDiscount()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Items = new List<OrderItemCommand> { new() { ProductId = 1, Quantity = 2 } },
                CouponId = 2,
                CustomerId = 1,
                AddressId = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var product = new Product { Id = 1, Price = 50.00m, Name = "Test Product" };
            var coupon = new Coupon 
            { 
                Id = 2, 
                DiscountAmount = 10.00m, // 10%
                DiscountType = DiscountType.Percentage // ← Importante: define o tipo
            };
            var order = new Order
            {
                Id = 1,
                SubTotal = new Money(100m), // Será alterado pelo handler
                OrderItems = new List<OrderItem>(),
                CustomerId = 1,
                AddressId = 1
            };
            var expectedOrderDto = new OrderDTO { Id = 1, SubTotal = 90m, CouponId = 2 }; // 100 - (100 * 0.10)

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

            _orderFactory.Setup(f => f.Create()).Returns(order);
            _productRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _couponRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(coupon);
            _orderRepository.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _mapper.Setup(m => m.Map<OrderDTO>(It.IsAny<Order>())).Returns(expectedOrderDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(90m, result.SubTotal);
            Assert.Equal(2, result.CouponId);

            _couponRepository.Verify(r => r.GetByIdAsync(2), Times.Once);
            _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentProduct_ShouldThrowException()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Items = new List<OrderItemCommand> { new() { ProductId = 999, Quantity = 2 } },
                CustomerId = 1,
                AddressId = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var order = new Order { OrderItems = new List<OrderItem>() };

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);

            _orderFactory.Setup(f => f.Create()).Returns(order);
            _productRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Product 999 not found", exception.Message);
        }
    }
}
