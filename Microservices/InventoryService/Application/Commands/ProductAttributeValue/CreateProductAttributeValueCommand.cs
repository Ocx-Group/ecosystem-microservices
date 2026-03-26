using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;

public record CreateProductAttributeValueCommand : IRequest<ProductAttributeValueDto?>
{
    public long IdAttribute { get; init; }
    public string AttributeValue { get; init; } = null!;
    public int Position { get; init; }
}
