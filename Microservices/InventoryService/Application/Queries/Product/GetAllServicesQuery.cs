using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetAllServicesQuery() : IRequest<ICollection<ProductDto>>;
