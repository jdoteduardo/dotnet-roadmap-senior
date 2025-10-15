namespace Week01_EFCore.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
