using ECommerce.OrderManagement.API.Tests.Repositories;
using ECommerce.OrderManagement.Domain.Entities;

namespace ECommerce.OrderManagement.Tests.Repositories
{
    public class AddressRepositoryTests : BaseRepositoryTest
    {
        [Fact]
        public async Task AddAddress_ShouldPersistToDatabase()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository<Address>(context);
            var address = new Address 
            { 
                Street = "Rua Teste", 
                City = "Cidade", 
                State = "SP", 
                ZipCode = "01234-567",
                Country = "BR"
            };

            // Act
            await repository.AddAsync(address);
            var addresses = await repository.GetAllAsync();

            // Assert
            Assert.Single(addresses);
            Assert.Equal("Rua Teste", addresses.First().Street);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository<Address>(context);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}