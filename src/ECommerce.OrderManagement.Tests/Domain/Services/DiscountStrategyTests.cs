using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Enums;
using ECommerce.OrderManagement.Domain.ValueObjects;
using ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Domain.Services
{
    public class DiscountStrategyTests
    {
        [Fact]
        public void FixedDiscountStrategy_ShouldReturnFixedAmount()
        {
            // Arrange
            var strategy = new FixedDiscountStrategy();
            var order = new Order { SubTotal = new Money(100.00m) };
            var coupon = new Coupon { DiscountAmount = 15.00m, DiscountType = DiscountType.Fixed };

            // Act
            var discount = strategy.CalculateDiscount(order, coupon);

            // Assert
            Assert.Equal(15.00m, discount);
        }

        [Fact]
        public void PercentageDiscountStrategy_ShouldReturnPercentageOfSubTotal()
        {
            // Arrange
            var strategy = new PercentageDiscountStrategy();
            var order = new Order { SubTotal = new Money(200.00m) };
            var coupon = new Coupon { DiscountAmount = 10.00m, DiscountType = DiscountType.Percentage }; // 10%

            // Act
            var discount = strategy.CalculateDiscount(order, coupon);

            // Assert
            Assert.Equal(20.00m, discount);
        }
    }
}
