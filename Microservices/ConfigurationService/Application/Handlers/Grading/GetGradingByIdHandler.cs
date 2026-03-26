using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Grading;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Grading;

public class GetGradingByIdHandler : IRequestHandler<GetGradingByIdQuery, GradingDto?>
{
    private readonly IGradingRepository _gradingRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetGradingByIdHandler> _logger;

    public GetGradingByIdHandler(
        IGradingRepository gradingRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetGradingByIdHandler> logger)
    {
        _gradingRepository = gradingRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GradingDto?> Handle(GetGradingByIdQuery request, CancellationToken cancellationToken)
    {
        var grading = await _gradingRepository.GetGradingById(request.Id);
        if (grading is null) return null;

        return _mapper.Map<GradingDto>(grading);
    }
}
