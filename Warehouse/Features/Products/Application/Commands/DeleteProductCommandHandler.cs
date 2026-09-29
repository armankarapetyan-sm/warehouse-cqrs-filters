using MediatR;
using Warehouse.Caching;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Features.Products.Application.Commands;

namespace Warehouse.Features.Products.Application.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly AppDbContext _db;
    private readonly ProductListCache _cache;

    public DeleteProductCommandHandler(AppDbContext db, ProductListCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _db.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException();
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);
        _cache.Remove();
    }
}
