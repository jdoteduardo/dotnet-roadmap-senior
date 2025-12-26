using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Events;
using ECommerce.OrderManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void MarkAsCreated_ShouldSetStatusAndAddDomainEvent()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 100,
                SubTotal = new Money(250.00m)
            };

            // Act
            order.MarkAsCreated();  

            // Assert
            Assert.Equal("Created", order.Status);
            Assert.Single(order.DomainEvents);
            var domainEvent = order.DomainEvents.First() as OrderCreatedEvent;
            Assert.NotNull(domainEvent);
            Assert.Equal(1, domainEvent.OrderId);
            Assert.Equal(100, domainEvent.CustomerId);
            Assert.Equal(250.00m, domainEvent.TotalAmount);
        }

        [Fact]
        public void MarkAsPaid_ShouldSetStatusAndAddDomainEvent()
        {
            // Arrange
            var order = new Order
            {
                Id = 2,
                UserId = 200,
                SubTotal = new Money(100.00m),
                Status = "Created"
            };

            // Act
            order.MarkAsPaid();

            // Assert
            Assert.Equal("Paid", order.Status);
            Assert.Single(order.DomainEvents);
            var domainEvent = order.DomainEvents.First() as OrderPaidEvent;
            Assert.NotNull(domainEvent);
            Assert.Equal(2, domainEvent.OrderId);
            Assert.Equal(200, domainEvent.CustomerId);
            Assert.Equal(100.00m, domainEvent.AmountPaid);
        }

        [Fact]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var order = new Order { Id = 1, UserId = 1, SubTotal = new Money(50m) };
            order.MarkAsCreated();
            order.MarkAsPaid();

            // Act
            order.ClearDomainEvents();

            // Assert
            Assert.Empty(order.DomainEvents);
        }
    }
}
