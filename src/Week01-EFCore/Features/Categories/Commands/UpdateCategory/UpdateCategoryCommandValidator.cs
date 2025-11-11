using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Commands.UpdateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateCategoryCommandValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Produto inválido.")
                .MustAsync(CategoryExists).WithMessage("Produto não encontrado.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
                .MaximumLength(200).WithMessage("Nome da categoria não pode ter mais de 200 caracteres.");
        }

        private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null;
        }
    }
}