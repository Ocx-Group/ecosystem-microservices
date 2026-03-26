using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductDiscount;

public record GetAllProductDiscountsByProductIdQuery(int ProductId) : IRequest<ICollection<ProductDiscountDto>>;
