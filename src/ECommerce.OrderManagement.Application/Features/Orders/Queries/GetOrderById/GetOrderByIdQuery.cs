using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderDTO?>
    {
        public int Id { get; set; }
    }
}
