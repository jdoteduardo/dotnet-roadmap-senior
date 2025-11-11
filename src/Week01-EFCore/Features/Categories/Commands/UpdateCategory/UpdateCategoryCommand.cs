using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
