using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Products.Application.Commands;
using Warehouse.Features.Products.Application.Dtos;
using Warehouse.Features.Products.Application.Mapping;
using Warehouse.Features.Products.Application.Queries;
using Warehouse.Filters;

namespace Warehouse.Features.Products.Presentation;

[ApiController]
[Authorize]
[MapErrors]
[AddApiHeader]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ProductMapper _mapper;

    public ProductsController(IMediator mediator, ProductMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ServiceFilter(typeof(ProductListCacheFilter))]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        IReadOnlyList<ProductDto> products = await _mediator.Send(new GetProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [PositiveId]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        ProductDto product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(product);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] CreateProductDto body, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        AddProductCommand command = _mapper.ToCommand(body, userId);
        int id = await _mediator.Send(command, cancellationToken);
        ProductDto created = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = id }, created);
    }

    [HttpPost("{id}/stock")]
    [PositiveId]
    [ValidateModel]
    public async Task<IActionResult> AdjustStock(
        int id,
        [FromBody] AdjustStockDto body,
        CancellationToken cancellationToken)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _mediator.Send(new AdjustStockCommand(id, body.Delta, userId), cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [PositiveId]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _mediator.Send(new DeleteProductCommand(id, userId), cancellationToken);
        return NoContent();
    }
}
