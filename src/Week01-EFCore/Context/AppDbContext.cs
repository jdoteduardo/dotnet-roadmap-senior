using Microsoft.EntityFrameworkCore;
using Week01_EFCore.Entities;

namespace Week01_EFCore.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category entity configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                // One-to-many relationship between Category and Product
                entity.HasMany(e => e.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
            });

            // Order entity configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).IsRequired();

                // One-to-many relationship between Order and OrderItem
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);
            });

            // OrderItem entity configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId });

                // Many-to-one relationship between OrderItem and Product
                entity.HasOne(oi => oi.Product)
                      .WithMany(p => p.OrderItems);

                // Many-to-one relationship between OrderItem and Order
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems);
            });

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                // One-to-many relationship between Product and OrderItem
                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Product)
                      .HasForeignKey(oi => oi.ProductId);

                // Many-to-one relationship between Product and Category
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products);
            });
        }

    }
}
