using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Factories;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.Services;
using ECommerce.OrderManagement.Infrastructure.Persistence.Context;
using ECommerce.OrderManagement.Infrastructure.Persistence.Repositories;
using ECommerce.OrderManagement.Infrastructure.Persistence.UnitOfWork;
using ECommerce.OrderManagement.Infrastructure.Services.DiscountStrategies;

namespace ECommerce.OrderManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Factories
            services.AddTransient<IEntityFactory<Order>, OrderFactory>();

            // Strategies
            services.AddScoped<IDiscountStrategy, FixedDiscountStrategy>();
            services.AddScoped<IDiscountStrategy, PercentageDiscountStrategy>();

            return services;
        }
    }
}
