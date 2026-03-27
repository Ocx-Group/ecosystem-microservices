using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletRequest;
using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class GetAllWalletRequestsHandler : IRequestHandler<GetAllWalletRequestsQuery, IEnumerable<WalletRequestDto>>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllWalletRequestsHandler> _logger;

    public GetAllWalletRequestsHandler(
        IWalletRequestRepository walletRequestRepository,
        IMapper mapper,
        ITenantContext tenantContext,
        ILogger<GetAllWalletRequestsHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _mapper = mapper;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletRequestDto>> Handle(GetAllWalletRequestsQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRequestRepository.GetAllWalletsRequests(_tenantContext.TenantId);
        return _mapper.Map<IEnumerable<WalletRequestDto>>(response);
    }
}
