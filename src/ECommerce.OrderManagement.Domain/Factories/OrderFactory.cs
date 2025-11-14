using ECommerce.OrderManagement.Domain.Entities;

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
                SubTotal = 0m,
                OrderItems = new List<OrderItem>()
            };
        }
    }
}
