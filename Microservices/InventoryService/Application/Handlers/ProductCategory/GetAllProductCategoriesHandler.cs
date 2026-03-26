using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductCategory;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCategory;

public class GetAllProductCategoriesHandler
    : IRequestHandler<GetAllProductCategoriesQuery, ICollection<ProductCategoryDto>>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductCategoriesHandler> _logger;

    public GetAllProductCategoriesHandler(
        IProductCategoryRepository productCategoryRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllProductCategoriesHandler> logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductCategoryDto>> Handle(
        GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _productCategoryRepository.GetAllProductsCategories(_tenantContext.TenantId);
        return _mapper.Map<ICollection<ProductCategoryDto>>(categories);
    }
}
