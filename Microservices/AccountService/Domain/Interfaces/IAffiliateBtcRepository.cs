using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IAffiliateBtcRepository
{
    Task<AffiliatesBtc?> GetTrc20AddressByAffiliateIdAndNetworkId(int id, long brandId);
    Task<AffiliatesBtc> CreateAffiliateBtcAsync(AffiliatesBtc affiliateBtc);
    Task<AffiliatesBtc> UpdateAffiliateBtcAsync(AffiliatesBtc affiliateBtc);
    Task<List<AffiliatesBtc>> GetAllAffiliatesBtcByIdsAsync(long[] ids, long brandId);
    Task<AffiliatesBtc?> GetBnbAddressByAffiliateIdAndNetworkId(int id, long brandId);
}
