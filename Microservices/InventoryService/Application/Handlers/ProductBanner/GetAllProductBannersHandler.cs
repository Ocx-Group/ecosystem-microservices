using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductBanner;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductBanner;

public class GetAllProductBannersHandler
    : IRequestHandler<GetAllProductBannersQuery, ICollection<ProductBannerDto>>
{
    private readonly IProductBannerRepository _productBannerRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductBannersHandler> _logger;

    public GetAllProductBannersHandler(
        IProductBannerRepository productBannerRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllProductBannersHandler> logger)
    {
        _productBannerRepository = productBannerRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductBannerDto>> Handle(
        GetAllProductBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _productBannerRepository.GetAllProductsBanner(_tenantContext.TenantId);
        return _mapper.Map<ICollection<ProductBannerDto>>(banners);
    }
}
