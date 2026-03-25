using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class AffiliateAddressRepository : BaseRepository, IAffiliateAddressRepository
{
    public AffiliateAddressRepository(AccountServiceDbContext context) : base(context) { }

    public async Task<List<AffiliatesAddress>?> GetAffiliatesAddressAsync()
        => await Context.AffiliatesAddresses.AsNoTracking().ToListAsync();

    public async Task<List<AffiliatesAddress>?> GetAffiliateAddressByAffiliateIdAsync(int affiliateId)
        => await Context.AffiliatesAddresses.Where(e => e.AffiliateId == affiliateId).AsNoTracking().ToListAsync();

    public async Task<AffiliatesAddress?> GetAffiliateAddressByIdAsync(int id)
        => await Context.AffiliatesAddresses.Where(e => e.Id == id).FirstOrDefaultAsync();

    public async Task<AffiliatesAddress> UpdateAffiliateAddressByIdAsync(AffiliatesAddress affiliatesAddress)
    {
        affiliatesAddress.UpdatedAt = DateTime.Now;
        Context.AffiliatesAddresses.Update(affiliatesAddress);
        await Context.SaveChangesAsync();
        return affiliatesAddress;
    }

    public async Task<AffiliatesAddress> CreateAffiliateAddressByIdAsync(AffiliatesAddress affiliatesAddress)
    {
        affiliatesAddress.CreatedAt = DateTime.Now;
        affiliatesAddress.UpdatedAt = DateTime.Now;
        await Context.AffiliatesAddresses.AddAsync(affiliatesAddress);
        await Context.SaveChangesAsync();
        return affiliatesAddress;
    }
}
