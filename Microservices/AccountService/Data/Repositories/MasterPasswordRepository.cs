using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class MasterPasswordRepository(AccountServiceDbContext context) : BaseRepository(context), IMasterPasswordRepository
{
    public async Task<MasterPassword?> GetMasterPasswordByBrandId(int brandId)
        => await Context.MasterPasswords.FirstOrDefaultAsync(x => x.BrandId == brandId);
}
