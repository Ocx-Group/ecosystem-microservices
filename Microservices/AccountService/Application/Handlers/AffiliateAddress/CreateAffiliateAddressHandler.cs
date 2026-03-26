using AutoMapper;
using Ecosystem.AccountService.Application.Commands.AffiliateAddress;
using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.AffiliateAddress;

public class CreateAffiliateAddressHandler : IRequestHandler<CreateAffiliateAddressCommand, AffiliateAddressDto?>
{
    private readonly IAffiliateAddressRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAffiliateAddressHandler> _logger;

    public CreateAffiliateAddressHandler(
        IAffiliateAddressRepository repo,
        IMapper mapper,
        ILogger<CreateAffiliateAddressHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AffiliateAddressDto?> Handle(CreateAffiliateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<AffiliatesAddress>(request);
        var created = await _repo.CreateAffiliateAddressByIdAsync(entity);
        return _mapper.Map<AffiliateAddressDto>(created);
    }
}
