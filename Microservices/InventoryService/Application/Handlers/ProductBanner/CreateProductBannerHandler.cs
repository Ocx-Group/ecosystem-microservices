using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductBanner;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductBanner;

public class CreateProductBannerHandler : IRequestHandler<CreateProductBannerCommand, ProductBannerDto?>
{
    private readonly IProductBannerRepository _productBannerRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductBannerHandler> _logger;

    public CreateProductBannerHandler(
        IProductBannerRepository productBannerRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductBannerHandler> logger)
    {
        _productBannerRepository = productBannerRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductBannerDto?> Handle(
        CreateProductBannerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsBanner>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productBannerRepository.CreateProductsBannerAsync(entity);
        return _mapper.Map<ProductBannerDto>(created);
    }
}
