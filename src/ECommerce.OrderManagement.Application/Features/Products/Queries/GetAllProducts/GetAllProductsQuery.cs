using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
    {
    }
}