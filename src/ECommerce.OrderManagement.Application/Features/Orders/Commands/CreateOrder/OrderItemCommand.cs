namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class OrderItemCommand
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
