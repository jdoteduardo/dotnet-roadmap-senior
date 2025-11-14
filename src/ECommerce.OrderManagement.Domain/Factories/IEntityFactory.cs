namespace ECommerce.OrderManagement.Domain.Factories
{
    public interface IEntityFactory<T> where T : class
    {
        T Create();
    }
}
