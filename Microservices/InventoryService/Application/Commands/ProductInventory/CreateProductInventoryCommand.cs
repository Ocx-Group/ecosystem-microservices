using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductInventory;

public record CreateProductInventoryCommand : IRequest<ProductInventoryDto?>
{
    public long IdProduct { get; init; }
    public int Ingress { get; init; }
    public int Egress { get; init; }
    public string? Support { get; init; }
    public string? Note { get; init; }
    public short Type { get; init; }
    public int? IdCombination { get; init; }
}
