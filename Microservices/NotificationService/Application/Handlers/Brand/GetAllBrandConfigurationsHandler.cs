using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Application.Queries.Brand;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Brand;

public class GetAllBrandConfigurationsHandler
    : IRequestHandler<GetAllBrandConfigurationsQuery, ICollection<BrandConfigurationDto>>
{
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetAllBrandConfigurationsHandler(
        IBrandConfigurationRepository brandRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _brandRepository = brandRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<ICollection<BrandConfigurationDto>> Handle(
        GetAllBrandConfigurationsQuery request, CancellationToken cancellationToken)
    {
        // Tenant-scoped: only the current tenant's config (zero or one row).
        var brand = await _brandRepository.GetByBrandIdAsync(_tenantContext.TenantId);
        var list = brand is null ? new List<DTOs.BrandConfigurationDto>() :
            new List<DTOs.BrandConfigurationDto> { _mapper.Map<BrandConfigurationDto>(brand) };
        return list;
    }
}
