using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductInventory;

public record GetAllProductInventoriesQuery() : IRequest<ICollection<ProductInventoryDto>>;
