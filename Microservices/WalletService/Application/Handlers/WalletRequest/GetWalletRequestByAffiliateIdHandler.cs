using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletRequest;
using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class GetWalletRequestByAffiliateIdHandler : IRequestHandler<GetWalletRequestByAffiliateIdQuery, IEnumerable<WalletRequestDto>?>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletRequestByAffiliateIdHandler> _logger;

    public GetWalletRequestByAffiliateIdHandler(
        IWalletRequestRepository walletRequestRepository,
        IMapper mapper,
        ILogger<GetWalletRequestByAffiliateIdHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletRequestDto>?> Handle(GetWalletRequestByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRequestRepository.GetAllWalletRequestByAffiliateId(request.Id);
        return _mapper.Map<IEnumerable<WalletRequestDto>>(response);
    }
}
