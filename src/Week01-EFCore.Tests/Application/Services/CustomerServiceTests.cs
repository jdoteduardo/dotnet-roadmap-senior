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
        private readonly Mock<IRepository<Customer>> _customerRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepository = new Mock<IRepository<Customer>>();
            _mapper = new Mock<IMapper>();

            _customerService = new CustomerService(
                _customerRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe" },
                new Customer { Id = 2, Name = "Jane Smith" }
            };
            _customerRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(customers);
            _mapper.Setup(m => m.Map<IEnumerable<CustomerDTO>>(It.IsAny<IEnumerable<Customer>>()))
                .Returns((IEnumerable<Customer> src) => src.Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }));

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
