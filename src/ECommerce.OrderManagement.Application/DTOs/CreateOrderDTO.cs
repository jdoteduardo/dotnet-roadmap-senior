namespace ECommerce.OrderManagement.Application.DTOs
{
    public class CreateOrderDTO
    {
        public List<OrderItemDTO> Items { get; set; } = new();
        public int? CouponId { get; set; }
    }
}
