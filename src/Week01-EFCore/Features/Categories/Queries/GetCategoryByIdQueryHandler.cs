using AutoMapper;
using FluentValidation;
using MediatR;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.UpdateProduct;
using Week01_EFCore.Features.Products.Queries.GetProductById;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Categories.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDTO?>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCategoryByIdQuery> _validator;

        public GetCategoryByIdQueryHandler(IRepository<Category> categoryRepository, IMapper mapper, IValidator<GetCategoryByIdQuery> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _categoryRepository.GetByIdAsync(request.Id);
            return _mapper.Map<CategoryDTO?>(category);
        }
    }
}
