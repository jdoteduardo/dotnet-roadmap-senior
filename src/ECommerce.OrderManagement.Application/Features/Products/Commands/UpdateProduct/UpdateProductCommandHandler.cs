using AutoMapper;
using FluentValidation;
using MediatR;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDTO>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProductCommand> _validator;

        public UpdateProductCommandHandler(
            IRepository<Product> productRepository, 
            IMapper mapper, 
            IValidator<UpdateProductCommand> validator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingProduct = await _productRepository.GetByIdAsync(request.Id);
            if (existingProduct == null)
                throw new Exception($"Product {request.Id} not found");

            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                CategoryId = request.CategoryId,
                Price = request.Price,
                CreatedAt = existingProduct.CreatedAt
            };

            var updatedProduct = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductDTO>(updatedProduct);
        }
    }
}