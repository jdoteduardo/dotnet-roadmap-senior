namespace Week01_EFCore.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
