using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductCombination;

public record GetProductCombinationsByProductIdQuery(int ProductId) : IRequest<ICollection<ProductCombinationDto>>;
