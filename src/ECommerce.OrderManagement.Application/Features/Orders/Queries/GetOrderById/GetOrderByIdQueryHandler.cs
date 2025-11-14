using AutoMapper;
using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO?>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetOrderByIdQuery> _validator;

        public GetOrderByIdQueryHandler(
            IRepository<Order> orderRepository, 
            IMapper mapper, 
            IValidator<GetOrderByIdQuery> validator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<OrderDTO?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var order = await _orderRepository.GetByIdAsync(request.Id);
            return _mapper.Map<OrderDTO?>(order);
        }
    }
}
