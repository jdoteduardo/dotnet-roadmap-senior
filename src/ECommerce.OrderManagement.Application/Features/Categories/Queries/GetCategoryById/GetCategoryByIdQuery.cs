using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CategoryDTO?>
    {
        public int Id { get; set; }
    }
}