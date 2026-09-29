using MediatR;
using Warehouse.Features.Products.Application.Dtos;

namespace Warehouse.Features.Products.Application.Queries;

public class GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>
{
}
