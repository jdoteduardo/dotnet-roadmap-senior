using ECommerce.OrderManagement.API.Tests.Repositories;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.ValueObjects;

namespace ECommerce.OrderManagement.Tests.Repositories
{
    public class CustomerRepositoryTests : BaseRepositoryTest
    {
        [Fact]
        public async Task AddCustomer_WithValidEmail_ShouldPersistToDatabase()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository<Customer>(context);
            var customer = new Customer 
            { 
                Name = "João Silva",
                Email = new Email("joao@teste.com")
            };

            // Act
            await repository.AddAsync(customer);
            var customers = await repository.GetAllAsync();

            // Assert
            Assert.Single(customers);
            Assert.Equal("João Silva", customers.First().Name);
            Assert.Equal("joao@teste.com", customers.First().Email.Value);
        }
    }
}