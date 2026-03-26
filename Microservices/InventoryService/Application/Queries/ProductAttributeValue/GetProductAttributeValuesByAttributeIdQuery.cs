using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductAttributeValue;

public record GetProductAttributeValuesByAttributeIdQuery(int AttributeId) : IRequest<ICollection<ProductAttributeValueDto>>;
