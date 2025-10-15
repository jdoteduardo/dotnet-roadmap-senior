using Week01_EFCore.Entities;
using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Factories
{
    public class CategoryFactory : IEntityFactory<Category>
    {
        public Category Create()
        {
            return new Category
            {
                Name = string.Empty,
                Products = new List<Product>()
            };
        }
    }
}