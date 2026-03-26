using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Grading;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Grading;

public class DeleteGradingHandler : IRequestHandler<DeleteGradingCommand, GradingDto?>
{
    private readonly IGradingRepository _gradingRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteGradingHandler> _logger;

    public DeleteGradingHandler(
        IGradingRepository gradingRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeleteGradingHandler> logger)
    {
        _gradingRepository = gradingRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GradingDto?> Handle(DeleteGradingCommand request, CancellationToken cancellationToken)
    {
        var grading = await _gradingRepository.GetGradingById(request.Id);
        if (grading is null) return null;

        var deleted = await _gradingRepository.DeleteGrading(grading);
        return _mapper.Map<GradingDto>(deleted);
    }
}
