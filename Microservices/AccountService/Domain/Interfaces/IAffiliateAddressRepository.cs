using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IAffiliateAddressRepository
{
    Task<List<AffiliatesAddress>?> GetAffiliatesAddressAsync();
    Task<List<AffiliatesAddress>?> GetAffiliateAddressByAffiliateIdAsync(int affiliateId);
    Task<AffiliatesAddress?> GetAffiliateAddressByIdAsync(int id);
    Task<AffiliatesAddress> CreateAffiliateAddressByIdAsync(AffiliatesAddress affiliatesAddress);
    Task<AffiliatesAddress> UpdateAffiliateAddressByIdAsync(AffiliatesAddress affiliatesAddress);
}
