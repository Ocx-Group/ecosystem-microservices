using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.Product;

public record DeleteProductCommand(int Id) : IRequest<ProductDto?>;
