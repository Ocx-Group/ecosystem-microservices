using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetAffiliatesWithoutAuthorizationHandler
    : IRequestHandler<GetAffiliatesWithoutAuthorizationQuery, ICollection<UsersAffiliatesDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly IMapper _mapper;

    public GetAffiliatesWithoutAuthorizationHandler(IUserAffiliateInfoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ICollection<UsersAffiliatesDto>> Handle(GetAffiliatesWithoutAuthorizationQuery request, CancellationToken ct)
    {
        var affiliates = await _repo.GetUsersWithoutAuthorization();
        return _mapper.Map<ICollection<UsersAffiliatesDto>>(affiliates);
    }
}
