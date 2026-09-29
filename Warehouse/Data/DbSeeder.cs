using Warehouse.Domain;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Products.AnyAsync())
        {
            return;
        }

        db.Products.Add(new Product
        {
            Sku = "WH-1001",
            Name = "Euro pallet",
            Quantity = 40,
            UnitPrice = 18.50m,
            CreatedBy = "seed"
        });
        db.Products.Add(new Product
        {
            Sku = "WH-1002",
            Name = "Stretch wrap roll",
            Quantity = 120,
            UnitPrice = 6.20m,
            CreatedBy = "seed"
        });
        await db.SaveChangesAsync();
    }
}
