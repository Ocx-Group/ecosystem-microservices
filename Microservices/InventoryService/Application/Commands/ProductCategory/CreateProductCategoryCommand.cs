using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductCategory;

public record CreateProductCategoryCommand : IRequest<ProductCategoryDto?>
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int? Category { get; init; }
    public int Level { get; init; }
    public bool? State { get; init; }
    public bool DisplaySmallBanner { get; init; }
    public bool DisplayBigBanner { get; init; }
}
