using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Services;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<IRepository<Category>> _categoryRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTest()
        {
            _categoryRepository = new Mock<IRepository<Category>>();
            _mapper = new Mock<IMapper>();

            _categoryService = new CategoryService(
                _categoryRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };
            _categoryRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);
            _mapper.Setup(m => m.Map<IEnumerable<CategoryDTO>>(It.IsAny<IEnumerable<Category>>()))
                .Returns((IEnumerable<Category> src) => src.Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }));

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Electronics" };
            _categoryRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(category);
            _mapper.Setup(m => m.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns((Category src) => new CategoryDTO
                {
                    Id = src.Id,
                    Name = src.Name
                });

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldCreateCategory()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Id = 0, Name = "category test" };
            var category = new Category { Id = 1, Name = "category test" };
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns((CategoryDTO src) => new Category
                {
                    Id = src.Id,
                    Name = src.Name
                });
            _categoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);
            _mapper.Setup(m => m.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns((Category src) => new CategoryDTO
                {
                    Id = src.Id,
                    Name = src.Name
                });

            // Act
            var result = await _categoryService.CreateCategoryAsync(categoryDTO.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("category test", result.Name);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Id = 1, Name = "category test update" };
            var category = new Category { Id = 1, Name = "category test update" };
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryDTO>()))
                .Returns((CategoryDTO src) => new Category
                {
                    Id = src.Id,
                    Name = src.Name
                });
            _categoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);
            _mapper.Setup(m => m.Map<CategoryDTO>(It.IsAny<Category>()))
                .Returns((Category src) => new CategoryDTO
                {
                    Id = src.Id,
                    Name = src.Name
                });

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryDTO.Id, categoryDTO.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("category test update", result.Name);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldDeleteCategory()
        {
            // Arrange
            _categoryRepository.Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
