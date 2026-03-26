using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetPersonalNetworkHandler : IRequestHandler<GetPersonalNetworkQuery, List<AffiliatePersonalNetwork>>
{
    private readonly IUserAffiliateInfoRepository _repo;

    public GetPersonalNetworkHandler(IUserAffiliateInfoRepository repo) => _repo = repo;

    public Task<List<AffiliatePersonalNetwork>> Handle(GetPersonalNetworkQuery request, CancellationToken ct)
        => _repo.GetPersonalNetwork(request.UserId);
}
