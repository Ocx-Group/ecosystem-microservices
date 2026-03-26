using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Grading;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Grading;

public class CreateGradingHandler : IRequestHandler<CreateGradingCommand, GradingDto>
{
    private readonly IGradingRepository _gradingRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateGradingHandler> _logger;

    public CreateGradingHandler(
        IGradingRepository gradingRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateGradingHandler> logger)
    {
        _gradingRepository = gradingRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GradingDto> Handle(CreateGradingCommand request, CancellationToken cancellationToken)
    {
        var grading = _mapper.Map<Gradings>(request);
        grading.BrandId = _tenantContext.TenantId;

        var created = await _gradingRepository.CreateGrading(grading);
        return _mapper.Map<GradingDto>(created);
    }
}
