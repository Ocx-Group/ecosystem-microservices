using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductCategory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCategory;

public class CreateProductCategoryHandler : IRequestHandler<CreateProductCategoryCommand, ProductCategoryDto?>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCategoryHandler> _logger;

    public CreateProductCategoryHandler(
        IProductCategoryRepository productCategoryRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductCategoryHandler> logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCategoryDto?> Handle(
        CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsCategory>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productCategoryRepository.CreateProductsCategoryAsync(entity);
        return _mapper.Map<ProductCategoryDto>(created);
    }
}
