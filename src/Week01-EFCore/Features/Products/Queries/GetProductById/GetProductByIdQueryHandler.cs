using AutoMapper;
using FluentValidation;
using MediatR;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;
using Week01_EFCore.Features.Products.Commands.UpdateProduct;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO?>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetProductByIdQuery> _validator;

        public GetProductByIdQueryHandler(IRepository<Product> productRepository, IMapper mapper, IValidator<GetProductByIdQuery> validator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var product = await _productRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ProductDTO?>(product);
        }
    }
}
