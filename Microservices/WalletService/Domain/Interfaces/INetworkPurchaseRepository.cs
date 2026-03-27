using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface INetworkPurchaseRepository
{
    Task<List<NetworkPurchase>> GetNetworkPurchasesEcoPool(DateTime from, DateTime to);
    Task<List<(int Year, int Month, int TotalPurchases)>> GetPurchasesMadeInMyNetwork(HashSet<int> ids);
}