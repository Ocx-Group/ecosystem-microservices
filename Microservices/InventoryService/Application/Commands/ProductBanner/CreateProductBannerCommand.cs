using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductBanner;

public record CreateProductBannerCommand : IRequest<ProductBannerDto?>
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public bool? ViewInfo { get; init; }
    public bool Status { get; init; }
}
