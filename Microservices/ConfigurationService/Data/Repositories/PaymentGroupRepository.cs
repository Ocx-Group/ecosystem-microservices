using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class PaymentGroupRepository : BaseRepository, IPaymentGroupRepository
{
    public PaymentGroupRepository(ConfigurationServiceDbContext context) : base(context) { }

    public async Task<PaymentGroups> CreatePaymentGroups(PaymentGroups paymentGroups)
    {
        var today = DateTime.Now;
        paymentGroups.UpdatedAt = today;
        paymentGroups.CreatedAt = today;
        paymentGroups.Status = true;

        await Context.PaymentGroups.AddAsync(paymentGroups);
        await Context.SaveChangesAsync();

        return paymentGroups;
    }

    public Task<List<PaymentGroups>> GetAllPaymentGroups(long brandId)
        => Context.PaymentGroups.Where(x => x.BrandId == brandId).AsNoTracking().ToListAsync();

    public async Task<PaymentGroups> DeletePaymentGroup(PaymentGroups paymentGroups)
    {
        paymentGroups.DeletedAt = DateTime.Now;

        Context.PaymentGroups.Update(paymentGroups);
        await Context.SaveChangesAsync();

        return paymentGroups;
    }

    public Task<PaymentGroups?> GetPaymentGroupById(int id)
        => Context.PaymentGroups.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<PaymentGroups> UpdatePaymentGroup(PaymentGroups paymentGroups)
    {
        paymentGroups.UpdatedAt = DateTime.Now;

        Context.PaymentGroups.Update(paymentGroups);
        await Context.SaveChangesAsync();

        return paymentGroups;
    }
}
