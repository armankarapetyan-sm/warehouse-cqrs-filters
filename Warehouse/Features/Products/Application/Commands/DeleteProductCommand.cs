using MediatR;
using Warehouse.Features.Products.Application;

namespace Warehouse.Features.Products.Application.Commands;

public class DeleteProductCommand : IRequest, IUserCommand
{
    public int ProductId { get; }
    public string UserId { get; }

    public DeleteProductCommand(int productId, string userId)
    {
        ProductId = productId;
        UserId = userId;
    }
}
