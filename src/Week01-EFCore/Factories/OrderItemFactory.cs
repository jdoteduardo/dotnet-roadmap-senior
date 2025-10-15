using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Factories
{
    public class OrderItemFactory : IEntityFactory<OrderItem>
    {
        public OrderItem Create()
        {
            return new OrderItem
            {
                OrderId = 0,
                ProductId = 0,
                Quantity = 0
            };
        }
    }
}
