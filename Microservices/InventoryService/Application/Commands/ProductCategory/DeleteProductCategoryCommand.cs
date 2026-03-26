using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductCategory;

public record DeleteProductCategoryCommand(int Id) : IRequest<ProductCategoryDto?>;
