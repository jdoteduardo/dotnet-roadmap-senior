using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week01_EFCore.Context;
using Week01_EFCore.Entities;
using Week01_EFCore.Repository;

namespace Week01_EFCore.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;

        public ProductRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;


            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        [Fact]
        public async Task AddProductWithoutCategory_ShouldThrowException()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repository = new Repository<Product>(context, uow);
            var product = new Product { Name = "Test Product", CategoryId = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await repository.AddAsync(product));
        }

        [Fact]
        public async Task AddProductWithCategoryExists_ShouldAddProductToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryCategory = new Repository<Category>(context, uow);
            var repositoryProduct = new Repository<Product>(context, uow);
            var category = new Category { Name = "Test Category" };
            await repositoryCategory.AddAsync(category);
            var product = new Product { Name = "Test Product", CategoryId = category.Id };

            // Act
            await repositoryProduct.AddAsync(product);
            var products = await repositoryProduct.GetAllAsync();

            // Assert
            Assert.Single(products);
            Assert.Equal("Test Product", products.First().Name);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryCategory = new Repository<Category>(context, uow);
            var repositoryProduct = new Repository<Product>(context, uow);
            var category = new Category { Name = "Test Category" };
            await repositoryCategory.AddAsync(category);
            var product1 = new Product { Name = "Test Product 1", CategoryId = category.Id };
            var product2 = new Product { Name = "Test Product 2", CategoryId = category.Id };
            await repositoryProduct.AddAsync(product1);
            await repositoryProduct.AddAsync(product2);

            // Act
            var products = await repositoryProduct.GetAllAsync();

            // Assert
            Assert.Equal(2, products.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryCategory = new Repository<Category>(context, uow);
            var repositoryProduct = new Repository<Product>(context, uow);
            var category = new Category { Name = "Test Category" };
            await repositoryCategory.AddAsync(category);
            var product = new Product { Name = "Test Product 1", CategoryId = category.Id };
            await repositoryProduct.AddAsync(product);

            // Act
            var result = await repositoryProduct.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var productId = 999;
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryProduct = new Repository<Product>(context, uow);

            // Act
            var result = await repositoryProduct.GetByIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldModifyExistingProduct()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryCategory = new Repository<Category>(context, uow);
            var repositoryProduct = new Repository<Product>(context, uow);
            var category = new Category { Name = "Test Category" };
            await repositoryCategory.AddAsync(category);
            var product = new Product { Name = "Test Product", CategoryId = category.Id };
            await repositoryProduct.AddAsync(product);

            // Act
            product.Name = "Updated Product";
            await repositoryProduct.UpdateAsync(product);
            var updatedProduct = await repositoryProduct.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct_WhenExists()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);
            var repositoryCategory = new Repository<Category>(context, uow);
            var repositoryProduct = new Repository<Product>(context, uow);
            var category = new Category { Name = "Test Category" };
            await repositoryCategory.AddAsync(category);
            var product = new Product { Name = "Test Product", CategoryId = category.Id };
            await repositoryProduct.AddAsync(product);

            // Act
            var result = await repositoryProduct.DeleteAsync(product.Id);
            var deletedProduct = await repositoryProduct.GetByIdAsync(product.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedProduct);
        }
    }
}
