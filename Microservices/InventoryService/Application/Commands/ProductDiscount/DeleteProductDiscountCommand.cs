using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.ProductDiscount;

public record DeleteProductDiscountCommand(int Id) : IRequest<ProductDiscountDto?>;
