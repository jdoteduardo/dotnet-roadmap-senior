using ECommerce.OrderManagement.Domain.Entities;

namespace ECommerce.OrderManagement.Domain.Services
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(Order order, Coupon coupon);
    }
}
