using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetWalletsRequestHandler : IRequestHandler<GetWalletsRequestQuery, IEnumerable<WalletDto>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletsRequestHandler> _logger;

    public GetWalletsRequestHandler(
        IWalletRepository walletRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetWalletsRequestHandler> logger)
    {
        _walletRepository = walletRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletDto>> Handle(GetWalletsRequestQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRepository.GetWalletsRequest(request.UserId, _tenantContext.TenantId);
        return _mapper.Map<IEnumerable<WalletDto>>(response);
    }
}
