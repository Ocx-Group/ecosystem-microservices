using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductAttributeValue;

public record GetAllProductAttributeValuesQuery() : IRequest<ICollection<ProductAttributeValueDto>>;
