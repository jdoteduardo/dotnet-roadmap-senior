using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDTO?>
    {
        public int Id { get; set; }
    }
}
