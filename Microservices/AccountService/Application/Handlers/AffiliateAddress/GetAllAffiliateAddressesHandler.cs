using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using Ecosystem.AccountService.Application.Queries.AffiliateAddress;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateAddress;

public class GetAllAffiliateAddressesHandler : IRequestHandler<GetAllAffiliateAddressesQuery, IEnumerable<AffiliateAddressDto>?>
{
    private readonly IAffiliateAddressRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllAffiliateAddressesHandler> _logger;

    public GetAllAffiliateAddressesHandler(
        IAffiliateAddressRepository repo,
        IMapper mapper,
        ILogger<GetAllAffiliateAddressesHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AffiliateAddressDto>?> Handle(GetAllAffiliateAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _repo.GetAffiliatesAddressAsync();
        return addresses is null ? null : _mapper.Map<IEnumerable<AffiliateAddressDto>>(addresses);
    }
}
