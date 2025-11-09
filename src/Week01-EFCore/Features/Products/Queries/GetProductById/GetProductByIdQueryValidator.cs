using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Features.Products.Commands.UpdateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        private readonly IRepository<Product> _productRepository;

        public GetProductByIdQueryValidator(IRepository<Product> productRepository)
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