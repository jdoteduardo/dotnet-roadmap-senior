using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderDTO>
    {
        public List<OrderItemCommand> Items { get; set; } = new();
        public int? CouponId { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
    }
}
