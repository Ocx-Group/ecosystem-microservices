using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductAttribute;

public record CreateProductAttributeCommand : IRequest<ProductAttributeDto?>
{
    public int? Attribute { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public bool? Type { get; init; }
    public int? Position { get; init; }
    public string? Color { get; init; }
    public bool? State { get; init; }
}
