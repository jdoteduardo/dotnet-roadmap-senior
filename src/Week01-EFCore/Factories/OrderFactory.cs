using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Factories
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
