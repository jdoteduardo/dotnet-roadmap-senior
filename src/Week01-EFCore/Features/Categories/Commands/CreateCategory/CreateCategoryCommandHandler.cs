using AutoMapper;
using FluentValidation;
using MediatR;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDTO>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryCommand> _validator;

        public CreateCategoryCommandHandler(IRepository<Category> categoryRepository, IMapper mapper, IValidator<CreateCategoryCommand> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CategoryDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = new Category
            {
                Name = request.Name
            };

            var createdCategory = await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDTO>(createdCategory);
        }
    }
}