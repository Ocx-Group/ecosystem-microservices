using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.Product;

public record GetProductsByBrandQuery(
    long[]? ProductIds,
    int[]? PaymentGroupIds,
    bool? ProductType,
    bool? State,
    bool? Visible,
    bool? VisiblePublic,
    bool IncludeDeleted = false
) : IRequest<ICollection<ProductDto>>;
