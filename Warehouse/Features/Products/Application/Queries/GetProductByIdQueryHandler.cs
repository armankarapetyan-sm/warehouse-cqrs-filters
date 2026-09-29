using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Features.Products.Application.Dtos;
using Warehouse.Features.Products.Application.Mapping;
using Warehouse.Features.Products.Application.Queries;

namespace Warehouse.Features.Products.Application.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly AppDbContext _db;
    private readonly ProductMapper _mapper;

    public GetProductByIdQueryHandler(AppDbContext db, ProductMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException();
        }

        return _mapper.ToDto(product);
    }
}
