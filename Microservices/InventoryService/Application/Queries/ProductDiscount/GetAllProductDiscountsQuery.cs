using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductDiscount;

public record GetAllProductDiscountsQuery() : IRequest<ICollection<ProductDiscountDto>>;
