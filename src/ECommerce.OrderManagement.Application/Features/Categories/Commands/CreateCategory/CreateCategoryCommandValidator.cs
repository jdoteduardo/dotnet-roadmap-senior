using FluentValidation;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome da categoria é obrigatório.")
                .MaximumLength(200).WithMessage("Nome da categoria não pode ter mais de 200 caracteres.");
        }
    }
}