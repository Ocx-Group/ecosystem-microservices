using Microsoft.Extensions.Options;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Configuration;

namespace Ecosystem.WalletService.Data.Repositories;

public class CreditRepository(IOptions<ApplicationConfiguration> appSettings, WalletServiceDbContext context) : BaseRepository(context), ICreditRepository
{
    
    public async Task<Credit> CreateCredit(Credit credit)
    {
        var today = DateTime.Now;
        credit.CreatedAt = today;
        credit.UpdatedAt = today;

        await Context.Credits.AddAsync(credit);

        return credit;
    }
}