using AutoMapper;
using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Enums;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;

namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDTO>
    {
        private readonly IEntityFactory<Order> _orderFactory;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IEnumerable<IDiscountStrategy> _discountStrategies;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderCommand> _validator;

        public CreateOrderCommandHandler(
            IEntityFactory<Order> orderFactory,
            IRepository<Product> productRepository,
            IRepository<Coupon> couponRepository,
            IRepository<Order> orderRepository,
            IEnumerable<IDiscountStrategy> discountStrategies,
            IMapper mapper,
            IValidator<CreateOrderCommand> validator)
        {
            _orderFactory = orderFactory;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _orderRepository = orderRepository;
            _discountStrategies = discountStrategies;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var order = _orderFactory.Create();

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new Exception($"Product {item.ProductId} not found");

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                order.OrderItems.Add(orderItem);
                order.SubTotal += CalculateItemSubTotal(product.Price, item.Quantity);
            }

            if (request.CouponId.HasValue)
            {
                await ApplyCoupon(request, order);
            }

            var createdOrder = await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderDTO>(createdOrder);
        }

        private async Task ApplyCoupon(CreateOrderCommand request, Order order)
        {
            var coupon = await _couponRepository.GetByIdAsync(request.CouponId.Value)
                ?? throw new Exception($"Coupon {request.CouponId.Value} not found");

            var strategy = _discountStrategies.FirstOrDefault(s => 
                s.GetType().Name.Contains(coupon.DiscountType.ToString()))
                ?? throw new Exception($"Strategy for {coupon.DiscountType} not found");

            var discount = strategy.CalculateDiscount(order, coupon);

            order.CouponId = coupon.Id;
            order.SubTotal -= discount;
        }

        private decimal CalculateItemSubTotal(decimal price, int quantity)
        {
            return price * quantity;
        }
    }
}
