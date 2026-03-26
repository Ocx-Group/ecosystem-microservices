using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductBanner;

public record DeleteProductBannerCommand(int Id) : IRequest<ProductBannerDto?>;
