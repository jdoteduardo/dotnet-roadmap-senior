using ECommerce.OrderManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_WithValue_ShouldSetValueAndDefaultCurrency()
        {
            // Act
            var money = new Money(100.50m);

            // Assert
            Assert.Equal(100.50m, money.Value);
            Assert.Equal("BRL", money.Currency);
        }

        [Fact]
        public void Constructor_WithValueAndCurrency_ShouldSetBoth()
        {
            // Act
            var money = new Money(200.75m, "USD");

            // Assert
            Assert.Equal(200.75m, money.Value);
            Assert.Equal("USD", money.Currency);
        }

        [Fact]
        public void ToString_ShouldReturnFormattedCurrency()
        {
            // Arrange
            var money = new Money(150.00m);

            // Act
            var result = money.ToString();

            // Assert
            Assert.Contains("150", result);
        }
    }
}
