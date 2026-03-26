using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductCombination;

public record CreateProductCombinationCommand : IRequest<ProductCombinationDto?>
{
    public long IdProduct { get; init; }
    public long IdAttributes { get; init; }
    public string CodeRef { get; init; } = null!;
    public bool? DisplayBigBanner { get; init; }
}
