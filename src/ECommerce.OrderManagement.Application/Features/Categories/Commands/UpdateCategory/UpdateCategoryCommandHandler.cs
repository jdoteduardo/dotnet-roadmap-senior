using AutoMapper;
using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDTO>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCategoryCommand> _validator;

        public UpdateCategoryCommandHandler(
            IRepository<Category> categoryRepository, 
            IMapper mapper, 
            IValidator<UpdateCategoryCommand> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CategoryDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = new Category
            {
                Id = request.Id,
                Name = request.Name
            };

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return _mapper.Map<CategoryDTO>(updatedCategory);
        }
    }
}