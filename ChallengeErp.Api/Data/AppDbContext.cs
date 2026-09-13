using ChallengeErp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChallengeErp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Order> Orders { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Receipt> Receipts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        modelBuilder.Entity<Line>()
            .Property(line => line.UnitOfMeasure)
            .HasConversion<string>()
            .HasMaxLength(10);

        modelBuilder.Entity<Order>().HasData(
            new Order 
            { 
                Id = "OC-1001", 
                Supplier = "Vidrios del sureste", 
                Status = OrderStatus.Open 
            }
        );

        modelBuilder.Entity<Line>().HasData(
            new Line { Id = 1, OrderId = "OC-1001", Article = "Vidrio flotado 6 mm", UnitOfMeasure = OrderUnitOfMeasure.M2, Quantity = 100m, Price = 180.00m },
            new Line { Id = 2, OrderId = "OC-1001", Article = "Silicón estructural", UnitOfMeasure = OrderUnitOfMeasure.Pza, Quantity = 40m, Price = 95.00m },
            new Line { Id = 3, OrderId = "OC-1001", Article = "Perfil de aluminio 3 m", UnitOfMeasure = OrderUnitOfMeasure.Pza, Quantity = 25m, Price = 310.00m }
        );
    }
}