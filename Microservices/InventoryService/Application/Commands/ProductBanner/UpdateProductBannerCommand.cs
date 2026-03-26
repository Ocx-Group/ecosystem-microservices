using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductBanner;

public record UpdateProductBannerCommand(
    int Id, string? Title, string? Description, bool ViewInfo, DateTime Date, bool Status
) : IRequest<ProductBannerDto?>;
