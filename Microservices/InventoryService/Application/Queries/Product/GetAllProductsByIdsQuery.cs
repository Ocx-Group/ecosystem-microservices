using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetAllProductsByIdsQuery(long[] ProductIds) : IRequest<ICollection<ProductDto>>;
