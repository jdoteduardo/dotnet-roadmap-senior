using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Factories;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Services
{
    public class OrderService : IOrderService<Order>
    {
        private readonly OrderFactory _orderFactory;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(OrderFactory orderFactory, IRepository<Product> productRepository, IRepository<Coupon> couponRepository, IRepository<Order> orderRepository)
        {
            _orderFactory = orderFactory;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDTO createOrder)
        {
            var order = _orderFactory.Create();

            foreach (var item in createOrder.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new Exception($"Product {item.ProductId} not found");

                var orderItem = new OrderItem
                {
                    OrderId = item.OrderId,
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

            return order;
        }

        private async Task ApplyCoupon(CreateOrderDTO createOrder, Order order)
        {
            var coupon = await _couponRepository.GetByIdAsync(createOrder.CouponId.Value)
                                ?? throw new Exception($"Coupon {createOrder.CouponId.Value} not found");

            order.CouponId = coupon.Id;
            order.SubTotal -= coupon.DiscountAmount;
        }

        private decimal CalculateItemSubTotal(decimal price, int quantity)
        {
            return price * quantity;
        }
    }
}
