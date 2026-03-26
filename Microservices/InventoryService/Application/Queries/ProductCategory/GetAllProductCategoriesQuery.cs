using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Queries.ProductCategory;

public record GetAllProductCategoriesQuery() : IRequest<ICollection<ProductCategoryDto>>;
