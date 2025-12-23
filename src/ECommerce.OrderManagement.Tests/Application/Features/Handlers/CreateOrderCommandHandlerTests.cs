using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.OrderManagement.Application.Interfaces;
using FluentValidation;
using Moq;
using Xunit;

namespace ECommerce.OrderManagement.Tests.Application.Features.Handlers
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IValidator<CreateOrderCommand>> _validator;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _orderService = new Mock<IOrderService>();
            _mapper = new Mock<IMapper>();
            _validator = new Mock<IValidator<CreateOrderCommand>>();
            _handler = new CreateOrderCommandHandler(_orderService.Object, _mapper.Object, _validator.Object);
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
            var expectedOrder = new OrderDTO { Id = 1, SubTotal = 100m };

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(validationResult);
            _orderService.Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderDTO>()))
                        .ReturnsAsync(expectedOrder);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _orderService.Verify(s => s.CreateOrderAsync(It.IsAny<CreateOrderDTO>()), Times.Once);
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
    }
}