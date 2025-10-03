using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Week01_EFCore.Context;

namespace Week01_EFCore.Tests
{
    public class LoadingPerformanceTests
    {
        private readonly AppDbContext _context;

        public LoadingPerformanceTests(AppDbContext context)
        {
            _context = context;
        }

        public void ExecuteTests()
        {
            Console.WriteLine("=== Testes de Performance de Loading ===\n");
            
            // Teste Eager Loading
            var stopwatchEager = new Stopwatch();
            stopwatchEager.Start();
            
            var productsEager = _context.Products
                .Include(p => p.Category)
                .Include(p => p.OrderItems)
                    .ThenInclude(oi => oi.Order)
                .ToList();

            foreach (var product in productsEager)
            {
                var categoryName = product.Category?.Name;
                var orderCount = product.OrderItems?.Count;
            }

            stopwatchEager.Stop();
            
            Console.WriteLine("=== Resultados Eager Loading ===");
            Console.WriteLine($"Tempo total: {stopwatchEager.ElapsedMilliseconds}ms");
            Console.WriteLine($"Produtos carregados: {productsEager.Count}");
            Console.WriteLine($"Exemplo primeiro produto: {productsEager.FirstOrDefault()?.Name}");
            Console.WriteLine($"Sua categoria: {productsEager.FirstOrDefault()?.Category?.Name}");
            Console.WriteLine($"Número de pedidos: {productsEager.FirstOrDefault()?.OrderItems?.Count}\n");

            // Limpando o contexto para não interferir no próximo teste
            _context.ChangeTracker.Clear();

            // Teste Lazy Loading
            var stopwatchLazy = new Stopwatch();
            stopwatchLazy.Start();
            
            var productsLazy = _context.Products.ToList();
            foreach (var product in productsLazy)
            {
                var categoryName = product.Category?.Name;
                var orderCount = product.OrderItems?.Count;
            }
            
            stopwatchLazy.Stop();
            
            Console.WriteLine("=== Resultados Lazy Loading ===");
            Console.WriteLine($"Tempo total: {stopwatchLazy.ElapsedMilliseconds}ms");
            Console.WriteLine($"Produtos carregados: {productsLazy.Count}");
            Console.WriteLine($"Exemplo primeiro produto: {productsLazy.FirstOrDefault()?.Name}");
            Console.WriteLine($"Sua categoria: {productsLazy.FirstOrDefault()?.Category?.Name}");
            Console.WriteLine($"Número de pedidos: {productsLazy.FirstOrDefault()?.OrderItems?.Count}");

            Console.WriteLine("\n=== Comparação ===");
            Console.WriteLine($"Eager Loading: {stopwatchEager.ElapsedMilliseconds}ms");
            Console.WriteLine($"Lazy Loading: {stopwatchLazy.ElapsedMilliseconds}ms");
        }
    }
}