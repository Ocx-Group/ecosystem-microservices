using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductInventory;

public record UpdateProductInventoryCommand(
    int Id, int IdProduct, int Ingress, int Egress, string? Support, string? Note,
    byte Type, DateTime Date, int IdCombination
) : IRequest<ProductInventoryDto?>;
