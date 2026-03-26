using AutoMapper;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Application.Queries.Brand;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Brand;

public class GetAllBrandConfigurationsHandler
    : IRequestHandler<GetAllBrandConfigurationsQuery, ICollection<BrandConfigurationDto>>
{
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IMapper _mapper;

    public GetAllBrandConfigurationsHandler(IBrandConfigurationRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<BrandConfigurationDto>> Handle(
        GetAllBrandConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _brandRepository.GetAllAsync();
        return _mapper.Map<ICollection<BrandConfigurationDto>>(brands);
    }
}
