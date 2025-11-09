using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ProductDTO>
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
