using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletWithDrawalRepository:BaseRepository,IWalletWithDrawalRepository
{
    public WalletWithDrawalRepository(WalletServiceDbContext context) : base(context)
    {
    }
    public Task<List<WalletsWithdrawal>> GetAllWalletsWithdrawals()
        =>  Context.WalletsWithdrawals.AsNoTracking().ToListAsync();
    
    public  Task<WalletsWithdrawal?> GetWalletWithdrawalById(int id)
        =>  Context.WalletsWithdrawals.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<WalletsWithdrawal> CreateWalletWithdrawalAsync(WalletsWithdrawal request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        
        await Context.AddAsync(request);
        await Context.SaveChangesAsync();

        return request;
    } 
    public async Task<WalletsWithdrawal> UpdateWalletWithdrawalAsync(WalletsWithdrawal request)
    {
        var today = DateTime.Now;
        request.UpdatedAt = today;
        Context.WalletsWithdrawals.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }
    public async Task<WalletsWithdrawal> DeleteWalletWithdrawalAsync(WalletsWithdrawal request)
    {
        request.DeletedAt = DateTime.Now;

        Context.WalletsWithdrawals.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }
}