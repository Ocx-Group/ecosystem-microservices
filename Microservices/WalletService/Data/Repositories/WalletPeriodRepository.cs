using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletPeriodRepository : BaseRepository, IWalletPeriodRepository
{
    public WalletPeriodRepository(WalletServiceDbContext context) : base(context) { }

    public Task<List<WalletsPeriod>> GetAllWalletsPeriods()
        => Context.WalletsPeriods.AsNoTracking().ToListAsync();

    public Task<WalletsPeriod?> GetWalletPeriodById(int id)
        => Context.WalletsPeriods.FirstOrDefaultAsync(x => x.Id == id);


    public async Task<IEnumerable<WalletsPeriod>> CreateWalletPeriodAsync(IEnumerable<WalletsPeriod> request)
    {
        var today          = DateTime.Now;
        var createdPeriods = new List<WalletsPeriod>();

        foreach (var period in request)
        {
            period.CreatedAt = today;
            period.UpdatedAt = today;

            await Context.AddAsync(period);
            createdPeriods.Add(period);
        }

        await Context.SaveChangesAsync();

        return createdPeriods;
    }

    public async Task<IEnumerable<WalletsPeriod>> UpdateWalletPeriodsAsync(IEnumerable<WalletsPeriod> request)
    {
        var today          = DateTime.Now;
        var updatedPeriods = new List<WalletsPeriod>();

        foreach (var period in request)
        {
            period.CreatedAt = today;
            period.UpdatedAt = today;

            Context.WalletsPeriods.Update(period);
            updatedPeriods.Add(period);
        }

        await Context.SaveChangesAsync();

        return updatedPeriods;
    }


    public async Task<WalletsPeriod> DeleteWalletPeriodAsync(WalletsPeriod request)
    {
        request.DeletedAt = DateTime.Now;

        Context.WalletsPeriods.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }
}