using ECommerce.OrderManagement.Domain.Enums;

namespace ECommerce.OrderManagement.Domain.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; }
        public DiscountType DiscountType { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
