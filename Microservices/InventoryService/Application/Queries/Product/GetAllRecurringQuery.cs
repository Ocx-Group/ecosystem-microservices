using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetAllRecurringQuery() : IRequest<ICollection<ProductDto>>;
