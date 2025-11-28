using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(string name, int categoryId, decimal price);
        Task<ProductDTO> UpdateProductAsync(int id, string name, int categoryId, decimal price);
        Task<bool> DeleteProductAsync(int id);
    }
}