using FluentValidation;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public DeleteProductCommandValidator(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Produto inválido.")
                .MustAsync(ProductExists).WithMessage("Produto não encontrado.");
        }

        private async Task<bool> ProductExists(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null;
        }
    }
}