using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Grading;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Grading;

public class UpdateGradingHandler : IRequestHandler<UpdateGradingCommand, GradingDto?>
{
    private readonly IGradingRepository _gradingRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateGradingHandler> _logger;

    public UpdateGradingHandler(
        IGradingRepository gradingRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateGradingHandler> logger)
    {
        _gradingRepository = gradingRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GradingDto?> Handle(UpdateGradingCommand request, CancellationToken cancellationToken)
    {
        var grading = await _gradingRepository.GetGradingById(request.Id);
        if (grading is null) return null;

        grading.Name = request.Name;
        grading.Description = request.Description;
        grading.ScopeLevel = request.ScopeLevel;
        grading.IsInfinity = request.IsInfinity;
        grading.PersonalPurchases = request.PersonalPurchases;
        grading.PersonalPurchasesExact = request.PersonalPurchasesExact;
        grading.PurchasesNetwork = request.PurchasesNetwork;
        grading.BinaryVolume = request.BinaryVolume;
        grading.VolumePoints = request.VolumePoints;
        grading.VolumePointsNetwork = request.VolumePointsNetwork;
        grading.ChildrenLeftLeg = request.ChildrenLeftLeg;
        grading.ChildrenRightLeg = request.ChildrenRightLeg;
        grading.FrontByMatrix = request.FrontByMatrix;
        grading.FrontQualif1 = request.FrontQualif1;
        grading.FrontScore1 = request.FrontScore1;
        grading.Frontqualif2 = request.FrontQualif2;
        grading.FrontScore2 = request.FrontScore2;
        grading.Frontqualif3 = request.FrontQualif3;
        grading.FrontScore3 = request.FrontScore3;
        grading.ExactFrontRatings = request.ExactFrontRatings;
        grading.LeaderByMatrix = request.LeaderByMatrix;
        grading.NetworkLeaders = request.NetworkLeaders;
        grading.NetworkLeadersQualifier = request.NetworkLeadersQualifier;
        grading.Products = request.Products;
        grading.Affiliations = request.Affiliations;
        grading.HaveBoth = request.HaveBoth;
        grading.ActivateUserBy = request.ActivateUserBy;
        grading.Active = request.Active;
        grading.Status = request.Status;
        grading.NetworkScopeLevel = request.NetworkScopeLevel;
        grading.FullPeriod = request.FullPeriod;
        grading.BrandId = _tenantContext.TenantId;

        var updated = await _gradingRepository.UpdateGrading(grading);
        return _mapper.Map<GradingDto>(updated);
    }
}
