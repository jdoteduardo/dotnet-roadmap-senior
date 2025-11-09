using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
