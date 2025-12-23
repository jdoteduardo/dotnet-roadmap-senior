using ECommerce.OrderManagement.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Domain.Factories
{
    public class OrderFactoryTests
    {
        [Fact]
        public void Create_ShouldReturnOrderWithDefaultValues()
        {
            // Arrange
            var factory = new OrderFactory();

            // Act
            var order = factory.Create();

            // Assert
            Assert.NotNull(order);
            Assert.Equal(string.Empty, order.Status);
            Assert.Equal(0m, order.SubTotal.Value);
            Assert.NotNull(order.OrderItems);
            Assert.Empty(order.OrderItems);
            Assert.True(order.OrderDate <= DateTime.Now);
        }
    }
}
