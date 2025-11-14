using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IValidator<DeleteCategoryCommand> _validator;

        public DeleteCategoryCommandHandler(
            IRepository<Category> categoryRepository,
            IValidator<DeleteCategoryCommand> validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _categoryRepository.DeleteAsync(request.Id);
        }
    }
}