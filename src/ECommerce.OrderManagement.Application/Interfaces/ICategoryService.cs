using ECommerce.OrderManagement.Application.DTOs;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(string name);
        Task<CategoryDTO> UpdateCategoryAsync(int id, string name);
        Task<bool> DeleteCategoryAsync(int id);
    }
}