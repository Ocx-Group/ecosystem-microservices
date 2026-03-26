using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductDiscount;

public record CreateProductDiscountCommand : IRequest<ProductDiscountDto?>
{
    public long IdProduct { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int Qualification { get; init; }
    public decimal Percentage { get; init; }
    public bool? PointsQualify { get; init; }
    public bool? BinaryPoints { get; init; }
    public bool? Commissionable { get; init; }
    public decimal PCommissionable { get; init; }
    public decimal? PBinaryPoints { get; init; }
    public decimal PPointsQualify { get; init; }
}
