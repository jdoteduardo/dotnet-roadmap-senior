using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Week01_EFCore.Configs;
using Week01_EFCore.Context;
using Week01_EFCore.Entities;
using Week01_EFCore.Factories;
using Week01_EFCore.Filters;
using Week01_EFCore.Interfaces;
using Week01_EFCore.Repository;
using Week01_EFCore.Repository.Decorators;
using Week01_EFCore.Services;
using Week01_EFCore.Services.Decorators;
using Week01_EFCore.Strategies;
using Week01_EFCore.Tests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<IEntityFactory<Order>, OrderFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registre os tipos específicos de repositório
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<OrderItem>, Repository<OrderItem>>();
builder.Services.AddScoped<IRepository<Coupon>, Repository<Coupon>>();

builder.Services.AddScoped<IDiscountStrategy, FixedDiscountStrategy>();
builder.Services.AddScoped<IDiscountStrategy, PercentageDiscountStrategy>();
builder.Services.AddAutoMapper(typeof(MappingConfigs));
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Decorators - agora vai funcionar porque os tipos específicos estão registrados
builder.Services.Decorate<IRepository<Order>, RepositoryLoggingDecorator<Order>>();
builder.Services.Decorate<IRepository<Product>, RepositoryLoggingDecorator<Product>>();
builder.Services.Decorate<IRepository<Category>, RepositoryLoggingDecorator<Category>>();
builder.Services.Decorate<IRepository<OrderItem>, RepositoryLoggingDecorator<OrderItem>>();
builder.Services.Decorate<IRepository<Coupon>, RepositoryLoggingDecorator<Coupon>>();
builder.Services.Decorate<IOrderService, OrderServiceLoggingDecorator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var performanceTests = new LoadingPerformanceTests(context);
    //performanceTests.ExecuteTests();
}

app.Run();
