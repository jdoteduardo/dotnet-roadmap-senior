using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public DeleteCategoryCommandValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Categoria inválida.")
                .MustAsync(CategoryExists).WithMessage("Categoria não encontrada.");
        }

        private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null;
        }
    }
}