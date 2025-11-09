using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;

        public UpdateProductCommandValidator(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Produto inválido.")
                .MustAsync(ProductExists).WithMessage("Produto não encontrado.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome do produto é obrigatório.")
                .MaximumLength(200).WithMessage("Nome do produto não pode ter mais de 200 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId inválido.")
                .MustAsync(CategoryExists).WithMessage("Categoria não encontrada.");
        }

        private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return category != null;
        }

        private async Task<bool> ProductExists(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null;
        }
    }
}