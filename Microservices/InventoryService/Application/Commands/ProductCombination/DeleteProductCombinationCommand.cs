using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductCombination;

public record DeleteProductCombinationCommand(int Id) : IRequest<ProductCombinationDto?>;
