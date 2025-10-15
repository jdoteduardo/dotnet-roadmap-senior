namespace Week01_EFCore.Interfaces
{
    public interface IEntityFactory<T> where T : class
    {
        T Create();
    }
}
