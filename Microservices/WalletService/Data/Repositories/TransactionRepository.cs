using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Configuration;

namespace Ecosystem.WalletService.Data.Repositories;

public class TransactionRepository : BaseRepository, ITransactionRepository
{
    private readonly ApplicationConfiguration _appSettings;

    public TransactionRepository(IOptions<ApplicationConfiguration> appSettings,
        WalletServiceDbContext context) : base(context)
        => _appSettings = appSettings.Value;

    public Task<List<Transaction>> GetAllTransaction(long brandId)
        => Context.Transactions.Where(x => x.BrandId == brandId).AsNoTracking().ToListAsync();

    public Task<Transaction?> GetTransactionByIdTransaction(string idTransaction, long brandId)
        => Context.Transactions.FirstOrDefaultAsync(e =>
            e.IdTransaction == idTransaction && (brandId == 0 || e.BrandId == brandId));

    public Task<Transaction?> GetTransactionByTxnId(string idTransaction)
        => Context.Transactions.FirstOrDefaultAsync(e =>
            e.IdTransaction == idTransaction);
    public async Task<Transaction?> CreateTransaction(Transaction request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;

        await Context.AddAsync(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public async Task<int> GetLastTransactionId(long brandId)
    {
        var lastTransaction = await Context.Transactions
            .Where(x => x.PaymentMethod == "CoinPay" && x.BrandId == brandId)
            .OrderByDescending(t => t.Id).FirstOrDefaultAsync();
        return lastTransaction != null ? int.Parse(lastTransaction.IdTransaction) : 0;
    }

    public async Task<Transaction?> UpdateTransactionAsync(Transaction request)
    {
        var today = DateTime.Now;
        request.UpdatedAt = today;
        Context.Transactions.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public Task<List<Transaction>> GetAllUnconfirmedOrUnpaidTransactions(long brandId)
    {
        return Context.Transactions
            .Where(e => e.Status != 100 && DateTime.Now > e.CreatedAt.AddHours(3) && e.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Transaction>> GetAllWireTransfer(long brandId)
        => Context.Transactions.Where(x => x.PaymentMethod == "wire_transfer" && x.BrandId == brandId)
            .AsNoTracking().ToListAsync();

    public Task<Transaction?> GetPaymentTransactionById(int id, long brandId)
        => Context.Transactions.FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public Task<Transaction?> GetTransactionByReference(string reference)
        => Context.Transactions.FirstOrDefaultAsync(e => e.Reference == reference);
}