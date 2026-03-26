using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductInventory;

public record DeleteProductInventoryCommand(int Id) : IRequest<ProductInventoryDto?>;
