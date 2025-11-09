using MediatR;
using Week01_EFCore.DTOs;

namespace Week01_EFCore.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
