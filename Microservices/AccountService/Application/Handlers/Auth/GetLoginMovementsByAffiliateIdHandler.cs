using Ecosystem.AccountService.Application.Queries.Auth;
using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Interfaces;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class GetLoginMovementsByAffiliateIdHandler : IRequestHandler<GetLoginMovementsByAffiliateIdQuery, List<LoginMovementsDto>>
{
    private readonly ILoginMovementsRepository _loginMovementsRepository;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;

    public GetLoginMovementsByAffiliateIdHandler(
        ILoginMovementsRepository loginMovementsRepository,
        IBrandService brandService,
        IMapper mapper)
    {
        _loginMovementsRepository = loginMovementsRepository;
        _brandService = brandService;
        _mapper = mapper;
    }

    public async Task<List<LoginMovementsDto>> Handle(GetLoginMovementsByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var loginMovements = await _loginMovementsRepository.GetLoginMovementsByAffiliateId(
            request.AffiliateId, _brandService.BrandId);
        return _mapper.Map<List<LoginMovementsDto>>(loginMovements);
    }
}
