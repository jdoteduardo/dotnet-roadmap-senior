namespace Week01_EFCore.DTOs
{
    public class CreateOrderDTO
    {
        public List<OrderItemDTO> Items { get; set; }
        public int? CouponId { get; set; }
    }
}
