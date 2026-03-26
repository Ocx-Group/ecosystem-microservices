using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductBanner;

public record GetAllProductBannersQuery() : IRequest<ICollection<ProductBannerDto>>;
