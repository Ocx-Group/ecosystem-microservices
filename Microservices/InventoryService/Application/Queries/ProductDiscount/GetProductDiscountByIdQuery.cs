using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductDiscount;

public record GetProductDiscountByIdQuery(int Id) : IRequest<ProductDiscountDto?>;
