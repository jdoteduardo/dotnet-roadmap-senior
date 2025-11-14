using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(string name, int categoryId, decimal price);
        Task<ProductDTO> UpdateProductAsync(int id, string name, int categoryId, decimal price);
        Task<bool> DeleteProductAsync(int id);
    }
}