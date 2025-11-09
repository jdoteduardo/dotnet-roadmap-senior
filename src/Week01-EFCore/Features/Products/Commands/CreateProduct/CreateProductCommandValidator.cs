using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateProductCommandValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

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
    }
}