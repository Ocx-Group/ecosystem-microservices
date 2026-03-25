using Ecosystem.AccountService.Application.Queries.Auth;
using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class GetLoginMovementsByAffiliateIdHandler : IRequestHandler<GetLoginMovementsByAffiliateIdQuery, List<LoginMovementsDto>>
{
    private readonly ILoginMovementsRepository _loginMovementsRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public GetLoginMovementsByAffiliateIdHandler(
        ILoginMovementsRepository loginMovementsRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _loginMovementsRepository = loginMovementsRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<List<LoginMovementsDto>> Handle(GetLoginMovementsByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var loginMovements = await _loginMovementsRepository.GetLoginMovementsByAffiliateId(
            request.AffiliateId, _tenantContext.TenantId);
        return _mapper.Map<List<LoginMovementsDto>>(loginMovements);
    }
}
