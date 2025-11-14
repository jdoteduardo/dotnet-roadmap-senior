using MediatR;
using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}