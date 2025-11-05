using Week01_EFCore.DTOs;

namespace Week01_EFCore.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO> UpdateProductAsync(ProductDTO productDTO);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDTO);
        Task<bool> DeleteProductAsync(int id);
    }
}
