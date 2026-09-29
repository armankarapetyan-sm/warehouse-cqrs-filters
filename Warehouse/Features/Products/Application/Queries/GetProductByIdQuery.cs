using MediatR;
using Warehouse.Features.Products.Application.Dtos;

namespace Warehouse.Features.Products.Application.Queries;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}
