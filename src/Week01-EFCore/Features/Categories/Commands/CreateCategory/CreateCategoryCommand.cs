using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDTO>
    {
        public string Name { get; set; } = string.Empty;
    }
}
