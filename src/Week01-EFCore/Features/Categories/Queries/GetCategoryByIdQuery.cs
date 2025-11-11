using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDTO?>
    {
        public int Id { get; set; }
    }
}
