using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using Ecosystem.AccountService.Application.Queries.AffiliateAddress;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateAddress;

public class GetAffiliateAddressByAffiliateIdHandler : IRequestHandler<GetAffiliateAddressByAffiliateIdQuery, IEnumerable<AffiliateAddressDto>?>
{
    private readonly IAffiliateAddressRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAffiliateAddressByAffiliateIdHandler> _logger;

    public GetAffiliateAddressByAffiliateIdHandler(
        IAffiliateAddressRepository repo,
        IMapper mapper,
        ILogger<GetAffiliateAddressByAffiliateIdHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AffiliateAddressDto>?> Handle(GetAffiliateAddressByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _repo.GetAffiliateAddressByAffiliateIdAsync(request.AffiliateId);
        return addresses is null ? null : _mapper.Map<IEnumerable<AffiliateAddressDto>>(addresses);
    }
}
