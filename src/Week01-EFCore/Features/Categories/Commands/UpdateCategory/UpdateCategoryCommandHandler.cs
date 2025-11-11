using AutoMapper;
using FluentValidation;
using MediatR;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Categories.Commands.CreateCategory;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDTO>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCategoryCommand> _validator;

        public UpdateCategoryCommandHandler(IRepository<Category> categoryRepository, IMapper mapper, IValidator<UpdateCategoryCommand> validator)
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