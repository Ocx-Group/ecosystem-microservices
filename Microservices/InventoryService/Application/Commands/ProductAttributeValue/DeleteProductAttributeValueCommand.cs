using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;

public record DeleteProductAttributeValueCommand(int Id) : IRequest<ProductAttributeValueDto?>;
