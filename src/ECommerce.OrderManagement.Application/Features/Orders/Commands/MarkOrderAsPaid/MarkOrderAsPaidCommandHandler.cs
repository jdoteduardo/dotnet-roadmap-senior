using MediatR;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.MarkOrderAsPaid
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, bool>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMediator _mediator;

        public MarkOrderAsPaidCommandHandler(IRepository<Order> orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            
            if (order == null)
                return false;

            order.MarkAsPaid();

            await _orderRepository.UpdateAsync(order);

            foreach (var domainEvent in order.DomainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            order.ClearDomainEvents();

            return true;
        }
    }
}