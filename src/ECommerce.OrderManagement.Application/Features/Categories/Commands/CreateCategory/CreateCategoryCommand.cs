using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDTO>
    {
        public string Name { get; set; } = string.Empty;
    }
}