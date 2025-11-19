using MediatR;

namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.MarkOrderAsPaid
{
    public class MarkOrderAsPaidCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
    }
}