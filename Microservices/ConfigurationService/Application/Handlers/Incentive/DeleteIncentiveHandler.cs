using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Incentive;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Incentive;

public class DeleteIncentiveHandler : IRequestHandler<DeleteIncentiveCommand, IncentiveDto?>
{
    private readonly IIncentiveRepository _incentiveRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteIncentiveHandler> _logger;

    public DeleteIncentiveHandler(
        IIncentiveRepository incentiveRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeleteIncentiveHandler> logger)
    {
        _incentiveRepository = incentiveRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IncentiveDto?> Handle(DeleteIncentiveCommand request, CancellationToken cancellationToken)
    {
        var incentive = await _incentiveRepository.GetIncentiveById(request.Id);
        if (incentive is null) return null;

        var deleted = await _incentiveRepository.DeleteIncentive(incentive);
        return _mapper.Map<IncentiveDto>(deleted);
    }
}
