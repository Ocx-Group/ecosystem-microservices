using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetPurchasesMadeInMyNetworkQuery(int AffiliateId) : IRequest<NetworkPurchasesResult?>;

public class NetworkPurchasesResult
{
    public List<PurchasesPerMonthDto> CurrentYearPurchases { get; set; } = new();
    public List<PurchasesPerMonthDto> PreviousYearPurchases { get; set; } = new();
}
