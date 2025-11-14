using FluentValidation;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public UpdateCategoryCommandValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Categoria inválida.")
                .MustAsync(CategoryExists).WithMessage("Categoria não encontrada.");

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