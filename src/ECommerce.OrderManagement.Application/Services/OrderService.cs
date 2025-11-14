using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Enums;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;

namespace ECommerce.OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IEntityFactory<Order> _orderFactory;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IEnumerable<IDiscountStrategy> _discountStrategies;
        private readonly IMapper _mapper;

        public OrderService(
            IEntityFactory<Order> orderFactory,
            IRepository<Product> productRepository, 
            IRepository<Coupon> couponRepository, 
            IRepository<Order> orderRepository,
            IEnumerable<IDiscountStrategy> discountStrategies,
            IMapper mapper)
        {
            _orderFactory = orderFactory;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _orderRepository = orderRepository;
            _discountStrategies = discountStrategies;
            _mapper = mapper;
        }

        public async Task<OrderDTO?> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO?>(order);
        }

        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createOrder)
        {
            var order = _orderFactory.Create();

            foreach (var item in createOrder.Items)
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

            if (createOrder.CouponId.HasValue)
            {
                await ApplyCoupon(createOrder, order);
            }

            await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderDTO>(order);
        }

        private async Task ApplyCoupon(CreateOrderDTO createOrder, Order order)
        {
            var coupon = await _couponRepository.GetByIdAsync(createOrder.CouponId.Value)
                                ?? throw new Exception($"Coupon {createOrder.CouponId.Value} not found");

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