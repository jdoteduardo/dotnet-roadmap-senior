using AutoMapper;
using FluentValidation;
using MediatR;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Categories.Commands.DeleteCategory;
using Week01_EFCore.Features.Products.Commands.CreateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Products.Commands.DeleteProduct
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<DeleteCategoryCommand> _validator;

        public DeleteCategoryCommandHandler(IRepository<Category> categoryRepository, IMapper mapper, IValidator<DeleteCategoryCommand> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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