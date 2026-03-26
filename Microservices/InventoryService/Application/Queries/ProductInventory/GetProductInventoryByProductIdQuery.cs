using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductInventory;

public record GetProductInventoryByProductIdQuery(int ProductId) : IRequest<ICollection<ProductInventoryDto>>;
