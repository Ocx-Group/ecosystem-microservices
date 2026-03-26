using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductCombination;

public record GetAllProductCombinationsQuery() : IRequest<ICollection<ProductCombinationDto>>;
