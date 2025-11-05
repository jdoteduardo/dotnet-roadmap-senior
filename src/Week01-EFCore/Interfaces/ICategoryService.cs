using Week01_EFCore.DTOs;

namespace Week01_EFCore.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO categoryDTO);
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
