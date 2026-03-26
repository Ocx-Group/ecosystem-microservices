using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductAttribute;

public record DeleteProductAttributeCommand(int Id) : IRequest<ProductAttributeDto?>;
