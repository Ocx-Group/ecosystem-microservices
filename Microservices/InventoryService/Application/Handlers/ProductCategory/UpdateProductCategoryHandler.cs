using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductCategory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCategory;

public class UpdateProductCategoryHandler : IRequestHandler<UpdateProductCategoryCommand, ProductCategoryDto?>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductCategoryHandler> _logger;

    public UpdateProductCategoryHandler(
        IProductCategoryRepository productCategoryRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductCategoryHandler> logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCategoryDto?> Handle(
        UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productCategoryRepository.GetProductsCategoriesById(request.Id);
        if (entity is null) return null;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Category = request.Category;
        entity.Level = request.Level;
        entity.State = request.State;
        entity.DisplaySmallBanner = request.DisplaySmallBanner;
        entity.DisplayBigBanner = request.DisplayBigBanner;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _productCategoryRepository.UpdateProductCategoryAsync(entity);
        return _mapper.Map<ProductCategoryDto>(updated);
    }
}
