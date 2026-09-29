using MediatR;
using Warehouse.Features.Products.Application;

namespace Warehouse.Features.Products.Application.Commands;

public class AdjustStockCommand : IRequest, IUserCommand
{
    public int ProductId { get; }
    public int Delta { get; }
    public string UserId { get; }

    public AdjustStockCommand(int productId, int delta, string userId)
    {
        ProductId = productId;
        Delta = delta;
        UserId = userId;
    }
}
