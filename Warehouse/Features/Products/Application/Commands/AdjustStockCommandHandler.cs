using MediatR;
using Warehouse.Caching;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Features.Products.Application.Commands;

namespace Warehouse.Features.Products.Application.Commands;

public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand>
{
    private readonly AppDbContext _db;
    private readonly ProductListCache _cache;

    public AdjustStockCommandHandler(AppDbContext db, ProductListCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _db.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException();
        }

        int next = product.Quantity + request.Delta;
        if (next < 0)
        {
            throw new InvalidOperationException("Stock cannot go below zero.");
        }

        product.Quantity = next;
        await _db.SaveChangesAsync(cancellationToken);
        _cache.Remove();
    }
}
