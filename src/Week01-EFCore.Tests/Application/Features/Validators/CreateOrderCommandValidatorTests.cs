using ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Features.Validators
{
    public class CreateOrderCommandValidatorTests
    {
        private readonly Mock<IRepository<Product>> _productRepository;
        private readonly Mock<IRepository<Customer>> _customerRepository;
        private readonly Mock<IRepository<Address>> _addressRepository;
        private readonly Mock<IRepository<Coupon>> _couponRepository;
        private readonly CreateOrderCommandValidator _validator;

        public CreateOrderCommandValidatorTests()
        {
            _productRepository = new Mock<IRepository<Product>>();
            _customerRepository = new Mock<IRepository<Customer>>();
            _addressRepository = new Mock<IRepository<Address>>();
            _couponRepository = new Mock<IRepository<Coupon>>();
            _validator = new CreateOrderCommandValidator(
                _productRepository.Object,
                _customerRepository.Object,
                _addressRepository.Object,
                _couponRepository.Object);
        }

        [Fact]
        public async Task Validate_WithValidData_ShouldBeValid()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                Items = new List<OrderItemCommand> { new() { ProductId = 1, Quantity = 2 } },
                CustomerId = 1,
                AddressId = 1
            };

            _productRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Product { Id = 1 });
            _customerRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Customer { Id = 1 });
            _addressRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Address { Id = 1 });

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validate_WithEmptyItems_ShouldBeInvalid()
        {
            // Arrange
            var command = new CreateOrderCommand { Items = new List<OrderItemCommand>() };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Items");
        }
    }
}
