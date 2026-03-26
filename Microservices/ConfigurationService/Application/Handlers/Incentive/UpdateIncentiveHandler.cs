using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Incentive;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Incentive;

public class UpdateIncentiveHandler : IRequestHandler<UpdateIncentiveCommand, IncentiveDto?>
{
    private readonly IIncentiveRepository _incentiveRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateIncentiveHandler> _logger;

    public UpdateIncentiveHandler(
        IIncentiveRepository incentiveRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateIncentiveHandler> logger)
    {
        _incentiveRepository = incentiveRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IncentiveDto?> Handle(UpdateIncentiveCommand request, CancellationToken cancellationToken)
    {
        var incentive = await _incentiveRepository.GetIncentiveById(request.Id);
        if (incentive is null) return null;

        incentive.Name = request.Name;
        incentive.Description = request.Description;
        incentive.ScopeLevel = request.ScopeLevel;
        incentive.IsInfinity = request.IsInfinity;
        incentive.PersonalPurchases = request.PersonalPurchases;
        incentive.PersonalPurchasesExact = request.PersonalPurchasesExact;
        incentive.PurchasesNetwork = request.PurchasesNetwork;
        incentive.BinaryVolume = request.BinaryVolume;
        incentive.VolumePoints = request.VolumePoints;
        incentive.VolumePointsNetwork = request.VolumePointsNetwork;
        incentive.ChildrenLeftLeg = request.ChildrenLeftLeg;
        incentive.ChildrenRightLeg = request.ChildrenRightLeg;
        incentive.FrontByMatrix = request.FrontByMatrix;
        incentive.FrontQualif1 = request.FrontQualif1;
        incentive.FrontScore1 = request.FrontScore1;
        incentive.FrontQuali2 = request.FrontQualif2;
        incentive.FrontScore2 = request.FrontScore2;
        incentive.FrontQualif3 = request.FrontQualif3;
        incentive.FrontScore3 = request.FrontScore3;
        incentive.ExactFrontRatings = request.ExactFrontRatings;
        incentive.LeaderByMatrix = request.LeaderByMatrix;
        incentive.NetworkLeaders = request.NetworkLeaders;
        incentive.NetworkLeadersQualifier = request.NetworkLeadersQualifier;
        incentive.Products = request.Products;
        incentive.Affiliations = request.Affiliations;
        incentive.Grading = request.Grading;
        incentive.Active = request.Active;
        incentive.Status = request.Status;
        incentive.BrandId = _tenantContext.TenantId;

        var updated = await _incentiveRepository.UpdateIncentive(incentive);
        return _mapper.Map<IncentiveDto>(updated);
    }
}
