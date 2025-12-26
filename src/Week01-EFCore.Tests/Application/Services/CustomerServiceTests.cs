using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Services;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Application.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IRepository<User>> _customerRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly UserService _customerService;

        public CustomerServiceTests()
        {
            _customerRepository = new Mock<IRepository<User>>();
            _mapper = new Mock<IMapper>();

            _customerService = new UserService(
                _customerRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnCustomers()
        {
            // Arrange
            var customers = new List<User>
            {
                new User { Id = 1, Name = "John Doe" },
                new User { Id = 2, Name = "Jane Smith" }
            };
            _customerRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(customers);
            _mapper.Setup(m => m.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>()))
                .Returns((IEnumerable<User> src) => src.Select(c => new UserDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }));

            // Act
            var result = await _customerService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
