using MediatR;
using Warehouse.Features.Products.Application;

namespace Warehouse.Features.Products.Application.Commands;

public class AddProductCommand : IRequest<int>, IUserCommand
{
    public string Sku { get; }
    public string Name { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public string UserId { get; }

    public AddProductCommand(string sku, string name, int quantity, decimal unitPrice, string userId)
    {
        Sku = sku;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
        UserId = userId;
    }
}
