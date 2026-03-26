using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Incentive;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Incentive;

public class GetAllIncentivesHandler : IRequestHandler<GetAllIncentivesQuery, ICollection<IncentiveDto>>
{
    private readonly IIncentiveRepository _incentiveRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllIncentivesHandler> _logger;

    public GetAllIncentivesHandler(
        IIncentiveRepository incentiveRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllIncentivesHandler> logger)
    {
        _incentiveRepository = incentiveRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<IncentiveDto>> Handle(GetAllIncentivesQuery request, CancellationToken cancellationToken)
    {
        var incentives = await _incentiveRepository.GetAllIncentive(_tenantContext.TenantId);
        return _mapper.Map<ICollection<IncentiveDto>>(incentives);
    }
}
