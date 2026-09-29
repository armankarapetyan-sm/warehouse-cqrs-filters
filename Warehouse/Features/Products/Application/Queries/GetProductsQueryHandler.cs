using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Features.Products.Application.Dtos;
using Warehouse.Features.Products.Application.Mapping;
using Warehouse.Features.Products.Application.Queries;

namespace Warehouse.Features.Products.Application.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly AppDbContext _db;
    private readonly ProductMapper _mapper;

    public GetProductsQueryHandler(AppDbContext db, ProductMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        List<Product> products = await _db.Products
            .AsNoTracking()
            .OrderBy(p => p.Sku)
            .ToListAsync(cancellationToken);
        return _mapper.ToDtoList(products);
    }
}
