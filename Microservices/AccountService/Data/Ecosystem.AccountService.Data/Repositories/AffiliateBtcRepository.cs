using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class AffiliateBtcRepository : BaseRepository, IAffiliateBtcRepository
{
    public AffiliateBtcRepository(AccountServiceDbContext context) : base(context) { }

    public Task<AffiliatesBtc?> GetTrc20AddressByAffiliateIdAndNetworkId(int id, long brandId)
        => Context.AffiliatesBtcs.OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(x => x.AffiliateId == id && x.BrandId == brandId && (x.NetworkId == 56 || x.NetworkId == 99));

    public Task<AffiliatesBtc?> GetBnbAddressByAffiliateIdAndNetworkId(int id, long brandId)
        => Context.AffiliatesBtcs.OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(x => x.AffiliateId == id && x.BrandId == brandId && x.NetworkId == 202);

    public async Task<AffiliatesBtc> CreateAffiliateBtcAsync(AffiliatesBtc affiliateBtc)
    {
        var today = DateTime.Now;
        affiliateBtc.UpdatedAt = today;
        affiliateBtc.CreatedAt = today;
        await Context.AffiliatesBtcs.AddAsync(affiliateBtc);
        await Context.SaveChangesAsync();
        return affiliateBtc;
    }

    public async Task<AffiliatesBtc> UpdateAffiliateBtcAsync(AffiliatesBtc affiliateBtc)
    {
        var today = DateTime.Now;
        affiliateBtc.UpdatedAt = today;
        affiliateBtc.CreatedAt = today;
        Context.AffiliatesBtcs.Update(affiliateBtc);
        await Context.SaveChangesAsync();
        return affiliateBtc;
    }

    public Task<List<AffiliatesBtc>> GetAllAffiliatesBtcByIdsAsync(long[] ids, long brandId)
        => Context.AffiliatesBtcs.Where(x => ids.Contains(x.AffiliateId) && x.BrandId == brandId).AsNoTracking().ToListAsync();
}
