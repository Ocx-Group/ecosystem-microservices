using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetPurchasesMadeInMyNetworkHandler : IRequestHandler<GetPurchasesMadeInMyNetworkQuery, NetworkPurchasesResult?>
{
    private readonly INetworkPurchaseRepository _networkPurchaseRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetPurchasesMadeInMyNetworkHandler> _logger;

    public GetPurchasesMadeInMyNetworkHandler(
        INetworkPurchaseRepository networkPurchaseRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ITenantContext tenantContext,
        ILogger<GetPurchasesMadeInMyNetworkHandler> logger)
    {
        _networkPurchaseRepository = networkPurchaseRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<NetworkPurchasesResult?> Handle(GetPurchasesMadeInMyNetworkQuery request, CancellationToken cancellationToken)
    {
        var affiliateId = request.AffiliateId;
        var brandId = _tenantContext.TenantId;

        var networkMembers = await _accountServiceAdapter.GetPersonalNetwork(affiliateId, brandId);

        if (networkMembers is null || !networkMembers.Any())
            return null;

        var idsInMyNetwork = new HashSet<int>(networkMembers.Select(affiliate => affiliate.id));
        idsInMyNetwork.Add(affiliateId);

        var purchasesSummary = await _networkPurchaseRepository.GetPurchasesMadeInMyNetwork(idsInMyNetwork);

        var groupedPurchases = purchasesSummary
            .GroupBy(p => p.Year)
            .ToDictionary(g => g.Key, g => g.Select(p => new PurchasesPerMonthDto
            {
                Year = p.Year,
                Month = p.Month,
                TotalPurchases = p.TotalPurchases
            }).ToList());

        var currentYear = DateTime.Now.Year;
        var previousYear = currentYear - 1;

        groupedPurchases.TryGetValue(currentYear, out var currentYearPurchases);
        groupedPurchases.TryGetValue(previousYear, out var previousYearPurchases);

        return new NetworkPurchasesResult
        {
            CurrentYearPurchases = currentYearPurchases ?? new List<PurchasesPerMonthDto>(),
            PreviousYearPurchases = previousYearPurchases ?? new List<PurchasesPerMonthDto>()
        };
    }
}
