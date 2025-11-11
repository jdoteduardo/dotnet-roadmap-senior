using FluentValidation;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Commands.CreateCategory
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