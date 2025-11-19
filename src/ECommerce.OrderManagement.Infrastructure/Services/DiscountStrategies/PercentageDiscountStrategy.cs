using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Services;

namespace ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order, Coupon coupon)
        {
            return order.SubTotal.Value * (coupon.DiscountAmount / 100);
        }
    }
}
