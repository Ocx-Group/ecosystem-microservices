using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetAllMembershipQuery() : IRequest<ICollection<ProductDto>>;
