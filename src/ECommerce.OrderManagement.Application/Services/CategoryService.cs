using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO?>(category);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(string name)
        {
            var category = new Category { Name = name };
            var createdCategory = await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDTO>(createdCategory);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, string name)
        {
            var category = new Category { Id = id, Name = name };
            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return _mapper.Map<CategoryDTO>(updatedCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}