using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
