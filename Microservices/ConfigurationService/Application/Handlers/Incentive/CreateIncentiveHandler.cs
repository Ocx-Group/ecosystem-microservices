using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Incentive;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Incentive;

public class CreateIncentiveHandler : IRequestHandler<CreateIncentiveCommand, IncentiveDto>
{
    private readonly IIncentiveRepository _incentiveRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateIncentiveHandler> _logger;

    public CreateIncentiveHandler(
        IIncentiveRepository incentiveRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateIncentiveHandler> logger)
    {
        _incentiveRepository = incentiveRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IncentiveDto> Handle(CreateIncentiveCommand request, CancellationToken cancellationToken)
    {
        var incentive = _mapper.Map<Incentives>(request);
        incentive.BrandId = _tenantContext.TenantId;

        var created = await _incentiveRepository.CreateIncentive(incentive);
        return _mapper.Map<IncentiveDto>(created);
    }
}
