using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class LoginMovementsRepository : BaseRepository, ILoginMovementsRepository
{
    public LoginMovementsRepository(AccountServiceDbContext context) : base(context) { }

    public Task<List<LoginMovement>> GetLoginMovementsByAffiliateId(int affiliateId, long brandId)
        => Context.LoginMovements.Where(x => x.AffiliateId == affiliateId && x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync();

    public async Task<LoginMovement> CreateAsync(LoginMovement loginMovement)
    {
        var today = DateTime.Now;
        loginMovement.CreatedAt = today;
        loginMovement.UpdatedAt = today;
        await Context.LoginMovements.AddAsync(loginMovement);
        await Context.SaveChangesAsync();
        return loginMovement;
    }

    public async Task<LoginMovement> UpdateAsync(LoginMovement loginMovement)
    {
        var today = DateTime.Now;
        loginMovement.CreatedAt = today;
        loginMovement.UpdatedAt = today;
        Context.LoginMovements.Update(loginMovement);
        await Context.SaveChangesAsync();
        return loginMovement;
    }
}
