using ChallengeErp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChallengeErp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Receipt> Receipts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        modelBuilder.Entity<Order>().HasData(
            new Order 
            { 
                Id = "OC-1001", 
                Provider = "Vidrios del Sureste", 
                Status = OrderStatus.Open 
            }
        );

        modelBuilder.Entity<OrderLine>().HasData(
            new OrderLine { Id = 1, OrderId = "OC-1001", Article = "Vidrio flotado 6 mm", Quantity = 100m, Price = 180.00m },
            new OrderLine { Id = 2, OrderId = "OC-1001", Article = "Silicón estructural", Quantity = 40m, Price = 95.00m },
            new OrderLine { Id = 3, OrderId = "OC-1001", Article = "Perfil de aluminio 3 m", Quantity = 25m, Price = 310.00m }
        );
    }
}