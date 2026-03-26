using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Grading;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Grading;

public class GetAllGradingsHandler : IRequestHandler<GetAllGradingsQuery, ICollection<GradingDto>>
{
    private readonly IGradingRepository _gradingRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllGradingsHandler> _logger;

    public GetAllGradingsHandler(
        IGradingRepository gradingRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllGradingsHandler> logger)
    {
        _gradingRepository = gradingRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<GradingDto>> Handle(GetAllGradingsQuery request, CancellationToken cancellationToken)
    {
        var gradings = await _gradingRepository.GetAllGrading(_tenantContext.TenantId);
        return _mapper.Map<ICollection<GradingDto>>(gradings);
    }
}
