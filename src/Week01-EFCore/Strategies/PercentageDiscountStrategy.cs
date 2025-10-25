using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Strategies
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order, Coupon coupon)
        {
            return order.SubTotal * (coupon.DiscountAmount / 100);
        }
    }
}
