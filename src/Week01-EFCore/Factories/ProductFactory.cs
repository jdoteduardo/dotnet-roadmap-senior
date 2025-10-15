using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Factories
{
    public class ProductFactory : IEntityFactory<Product>
    {
        public Product Create()
        {
            return new Product
            {
                Name = string.Empty,
                CategoryId = 0,
                CreatedAt = DateTime.Now,
                Price = 0m,
                OrderItems = new List<OrderItem>()
            };
        }
    }
}