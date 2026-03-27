using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.BonusRequest;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IBonusRepository
{
    Task<int> CreateBonus(BonusRequest request);
    Task<decimal> GetBonusAmountByAffiliateId(int affiliateId);
}