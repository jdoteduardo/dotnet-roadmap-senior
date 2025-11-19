using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.ValueObjects;

namespace ECommerce.OrderManagement.Domain.Factories
{
    public class OrderFactory : IEntityFactory<Order>
    {
        public Order Create()
        {
            return new Order
            {
                OrderDate = DateTime.Now,
                Status = string.Empty,
                SubTotal = new Money(0m),
                OrderItems = new List<OrderItem>()
            };
        }
    }
}
