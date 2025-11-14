using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IValidator<DeleteProductCommand> _validator;

        public DeleteProductCommandHandler(
            IRepository<Product> productRepository,
            IValidator<DeleteProductCommand> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _productRepository.DeleteAsync(request.Id);
        }
    }
}