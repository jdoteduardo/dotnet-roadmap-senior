namespace ECommerce.OrderManagement.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public int? CouponId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
    }
}
