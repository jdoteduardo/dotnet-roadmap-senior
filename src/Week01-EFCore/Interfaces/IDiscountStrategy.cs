using Week01_EFCore.Entities;

namespace Week01_EFCore.Interfaces
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(Order order, Coupon coupon);
    }
}
