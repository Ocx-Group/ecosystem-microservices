using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class InvoiceDetailRepository : BaseRepository, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(WalletServiceDbContext context) : base(context) { }
    public Task<List<InvoicesDetail>> GetAllInvoiceDetailAsync()
        => Context.InvoicesDetails.Where(x => x.DeletedAt == null).AsNoTracking().ToListAsync();

    public Task<List<InvoicesDetail>> GetInvoiceDetailByInvoiceIdAsync(long invoiceId)
        => Context.InvoicesDetails.Where(x => x.InvoiceId == invoiceId).ToListAsync();

    public async Task<InvoicesDetail> CreateInvoiceDetailAsync(InvoicesDetail invoice)
    {
        var today = DateTime.Now;
        invoice.CreatedAt = today;
        invoice.UpdatedAt = today;

        await Context.AddAsync(invoice);
        await Context.SaveChangesAsync();

        return invoice;
    }

    public async Task<List<InvoicesDetail>> CreateBulkInvoiceDetailAsync(List<InvoicesDetail> requests)
    {
        const int take         = 1000;
        var       packageCount = requests.Count / take;

        for (var i = 0; i < packageCount; i++)
        {
            var packageList = requests.Skip(i * take).Take(take).ToList();
            Context.InvoicesDetails.UpdateRange(packageList);
        }

        await Context.SaveChangesAsync();

        return requests;
    }
    public async Task<List<InvoicesDetail>> DeleteBulkInvoiceDetailAsync(List<InvoicesDetail> requests)
    {
        var today = DateTime.Now;
        foreach (var item in requests)
        {
            item.UpdatedAt = today;
            item.DeletedAt = today;
        }

        const int take         = 1000;
        var       packageCount = requests.Count / take;

        for (var i = 0; i < packageCount; i++)
        {
            var packageList = requests.Skip(i * take).Take(take).ToList();

            Context.InvoicesDetails.UpdateRange(packageList);
        }

        await Context.SaveChangesAsync();

        return requests;
    }

}