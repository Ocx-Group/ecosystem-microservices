using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetAllTradingAcademyQuery() : IRequest<ICollection<ProductDto>>;
