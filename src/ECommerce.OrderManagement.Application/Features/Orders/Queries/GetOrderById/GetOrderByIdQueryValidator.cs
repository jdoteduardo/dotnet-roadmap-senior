using FluentValidation;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        private readonly IRepository<Order> _orderRepository;

        public GetOrderByIdQueryValidator(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Pedido inválido.")
                .MustAsync(OrderExists).WithMessage("Pedido não encontrado.");
        }

        private async Task<bool> OrderExists(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order != null;
        }
    }
}
