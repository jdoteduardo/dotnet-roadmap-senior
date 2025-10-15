namespace Week01_EFCore.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
