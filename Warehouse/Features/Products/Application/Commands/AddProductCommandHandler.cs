using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Caching;
using Warehouse.Data;
using Warehouse.Domain;

namespace Warehouse.Features.Products.Application.Commands;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
{
    private readonly AppDbContext _db;
    private readonly ProductListCache _cache;

    public AddProductCommandHandler(AppDbContext db, ProductListCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Sku) || string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Sku and name are required.");
        }

        if (request.Quantity < 0 || request.UnitPrice < 0)
        {
            throw new InvalidOperationException("Quantity and price cannot be negative.");
        }

        bool exists = await _db.Products.AnyAsync(p => p.Sku == request.Sku, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Sku already exists.");
        }

        Product product = new Product();
        product.Sku = request.Sku.Trim();
        product.Name = request.Name.Trim();
        product.Quantity = request.Quantity;
        product.UnitPrice = request.UnitPrice;
        product.CreatedBy = request.UserId;
        _db.Products.Add(product);
        await _db.SaveChangesAsync(cancellationToken);
        _cache.Remove();
        return product.Id;
    }
}
