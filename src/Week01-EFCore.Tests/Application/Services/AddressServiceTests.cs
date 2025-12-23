using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Services;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;
using ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Services
{
    public class AddressServiceTests
    {
        private readonly Mock<IRepository<Address>> _addressRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly AddressService _addressService;

        public AddressServiceTests()
        {
            _addressRepository = new Mock<IRepository<Address>>();
            _mapper = new Mock<IMapper>();

            _addressService = new AddressService(
                _addressRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllAddressesAsync_ShouldReturnAddresses()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" },
                new Address { Id = 2, Street = "456 Oak St", City = "Othertown", State = "TX", ZipCode = "67890" }
            };
            _addressRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(addresses);
            _mapper.Setup(m => m.Map<IEnumerable<AddressDTO>>(It.IsAny<IEnumerable<Address>>()))
                .Returns((IEnumerable<Address> src) => src.Select(a => new AddressDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode
                }));

            // Act
            var result = await _addressService.GetAllAddressesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAddressByIdAsync_WithValidId_ShouldReturnAddress()
        {
            // Arrange
            var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" };
            _addressRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(address);
            _mapper.Setup(m => m.Map<AddressDTO>(It.IsAny<Address>()))
                .Returns((Address src) => new AddressDTO
                {
                    Id = src.Id,
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                });

            // Act
            var result = await _addressService.GetAddressByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateAddressAsync_ShouldCreateAndReturnAddress()
        {
            // Arrange
            var addressDTO = new AddressDTO { Id = 0, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" };
            var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" };
            _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressDTO>()))
                .Returns((AddressDTO src) => new Address
                {
                    Id = src.Id,
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                });
            _addressRepository.Setup(repo => repo.AddAsync(It.IsAny<Address>()))
                .ReturnsAsync(address);
            _mapper.Setup(m => m.Map<AddressDTO>(It.IsAny<Address>()))
                .Returns((Address src) => new AddressDTO
                {
                    Id = src.Id,
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                });

            // Act
            var result = await _addressService.CreateAddressAsync(addressDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateAddressAsync_ShouldUpdateAndReturnAddress()
        {
            // Arrange
            var addressDTO = new AddressDTO { Id = 1, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" };
            var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" };
            _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressDTO>()))
                .Returns((AddressDTO src) => new Address
                {
                    Id = src.Id,
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                });
            _addressRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Address>()))
                .ReturnsAsync(address);
            _mapper.Setup(m => m.Map<AddressDTO>(It.IsAny<Address>()))
                .Returns((Address src) => new AddressDTO
                {
                    Id = src.Id,
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                });

            // Act
            var result = await _addressService.UpdateAddressAsync(addressDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeleteAddressAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            _addressRepository.Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _addressService.DeleteAddressAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
