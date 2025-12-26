using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Application.Services;

namespace ECommerce.OrderManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            // AutoMapper
            services.AddAutoMapper(assembly);

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<JwtService>();

            return services;
        }
    }
}
