using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductAttribute;

public record GetAllProductAttributesQuery() : IRequest<ICollection<ProductAttributeDto>>;
