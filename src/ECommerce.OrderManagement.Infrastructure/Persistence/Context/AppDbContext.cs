using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ECommerce.OrderManagement.Domain.Entities;

namespace ECommerce.OrderManagement.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ECommerce.OrderManagement.API");
                
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category entity configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
            });

            // Order entity configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OrderDate).IsRequired();

                entity.OwnsOne(o => o.SubTotal, money =>
                {
                    money.Property(m => m.Value)
                        .HasColumnName("SubTotal")
                        .HasColumnType("decimal(18,2)");
                    money.Property(m => m.Currency)
                        .HasColumnName("Currency")
                        .HasMaxLength(3)
                        .IsRequired();
                });

                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);

                entity.HasOne(e => e.Coupon)
                      .WithMany(e => e.Orders);

                entity.HasOne(e => e.Address)
                      .WithMany(e => e.Orders);

                entity.HasOne(e => e.Customer)
                      .WithMany(e => e.Orders);
            });

            // OrderItem entity configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId });
                entity.Property(e => e.Quantity).HasColumnType("int");

                entity.HasOne(oi => oi.Product)
                      .WithMany(p => p.OrderItems);

                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems);
            });

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasMany(e => e.OrderItems)
                      .WithOne(oi => oi.Product)
                      .HasForeignKey(oi => oi.ProductId);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Products);

                entity.HasIndex(e => e.Name);
            });

            // Coupon entity configuration 
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CouponCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DiscountAmount).IsRequired().HasColumnType("decimal(5,2)");
                entity.Property(e => e.DiscountType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Coupon)
                      .HasForeignKey(o => o.CouponId);
            });

            // Address entity configuration
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Street).IsRequired().HasMaxLength(100);
                entity.Property(e => e.City).IsRequired().HasMaxLength(50);
                entity.Property(e => e.State).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(50);

                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Address)
                      .HasForeignKey(o => o.AddressId);
            });

            // Customer entity configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.OwnsOne(c => c.Email, email =>
                {
                    email.Property(e => e.Value)
                        .HasColumnName("Email")
                        .HasMaxLength(255)
                        .IsRequired();
                });

                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Customer)
                      .HasForeignKey(o => o.CustomerId);
            });
        }
    }
}
