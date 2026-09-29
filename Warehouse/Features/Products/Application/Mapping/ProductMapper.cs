using Warehouse.Domain;
using Warehouse.Features.Products.Application.Commands;
using Warehouse.Features.Products.Application.Dtos;

namespace Warehouse.Features.Products.Application.Mapping;

public class ProductMapper
{
    public AddProductCommand ToCommand(CreateProductDto dto, string userId)
    {
        return new AddProductCommand(dto.Sku, dto.Name, dto.Quantity, dto.UnitPrice, userId);
    }

    public ProductDto ToDto(Product product)
    {
        ProductDto dto = new ProductDto();
        dto.Id = product.Id;
        dto.Sku = product.Sku;
        dto.Name = product.Name;
        dto.Quantity = product.Quantity;
        dto.UnitPrice = product.UnitPrice;
        return dto;
    }

    public IReadOnlyList<ProductDto> ToDtoList(IReadOnlyList<Product> products)
    {
        List<ProductDto> list = new List<ProductDto>(products.Count);
        for (int i = 0; i < products.Count; i++)
        {
            list.Add(ToDto(products[i]));
        }

        return list;
    }
}
