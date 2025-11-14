using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Infrastructure.Persistence.Context;

namespace ECommerce.OrderManagement.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private bool _disposed;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_context.Database.ProviderName?.Contains("InMemory") == false)
            {
                await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            
            var transaction = _context.Database.CurrentTransaction;
            if (transaction != null)
            {
                await transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
