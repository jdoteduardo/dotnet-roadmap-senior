using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Services;

namespace ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies
{
    public class FixedDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order, Coupon coupon)
        {
            return coupon.DiscountAmount;
        }
    }
}
