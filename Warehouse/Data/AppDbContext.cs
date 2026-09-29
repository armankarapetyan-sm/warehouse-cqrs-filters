using Warehouse.Domain;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Sku).IsUnique();
            entity.Property(x => x.Sku).HasMaxLength(32).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(128).IsRequired();
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.Property(x => x.CreatedBy).HasMaxLength(64).IsRequired();
        });
    }
}
