using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetAllAffiliatesWithPositiveBalanceHandler : IRequestHandler<GetAllAffiliatesWithPositiveBalanceQuery, IEnumerable<AffiliateBalance>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllAffiliatesWithPositiveBalanceHandler> _logger;

    public GetAllAffiliatesWithPositiveBalanceHandler(
        IWalletRepository walletRepository,
        ITenantContext tenantContext,
        ILogger<GetAllAffiliatesWithPositiveBalanceHandler> logger)
    {
        _walletRepository = walletRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<AffiliateBalance>> Handle(GetAllAffiliatesWithPositiveBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _walletRepository.GetAllAffiliatesWithPositiveBalance(_tenantContext.TenantId);
    }
}
