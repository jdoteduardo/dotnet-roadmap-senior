using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ProductDTO>
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}