using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductBanner;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductBanner;

public class UpdateProductBannerHandler : IRequestHandler<UpdateProductBannerCommand, ProductBannerDto?>
{
    private readonly IProductBannerRepository _productBannerRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductBannerHandler> _logger;

    public UpdateProductBannerHandler(
        IProductBannerRepository productBannerRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductBannerHandler> logger)
    {
        _productBannerRepository = productBannerRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductBannerDto?> Handle(
        UpdateProductBannerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productBannerRepository.GetProductsBannerById(request.Id);
        if (entity is null) return null;

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ViewInfo = request.ViewInfo;
        entity.Date = request.Date;
        entity.Status = request.Status;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _productBannerRepository.UpdateProductBannerAsync(entity);
        return _mapper.Map<ProductBannerDto>(updated);
    }
}
