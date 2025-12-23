using ECommerce.OrderManagement.Infrastructure.Persistence.Context;
using ECommerce.OrderManagement.Infrastructure.Persistence.Repositories;
using ECommerce.OrderManagement.Infrastructure.Persistence.UnitOfWork;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Repositories
{
    public abstract class BaseRepositoryTest : IDisposable
    {
        protected readonly SqliteConnection _connection;
        protected readonly DbContextOptions<AppDbContext> _options;

        protected BaseRepositoryTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();
        }

        protected AppDbContext CreateContext() => new AppDbContext(_options);

        protected Repository<T> CreateRepository<T>(AppDbContext context) where T : class
        {
            var uow = new UnitOfWork(context);
            return new Repository<T>(context, uow);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
