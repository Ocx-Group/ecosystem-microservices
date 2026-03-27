using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletRequestRepository : BaseRepository, IWalletRequestRepository
{
    public WalletRequestRepository(WalletServiceDbContext context) : base(context)
    {
    }
    public Task<List<WalletsRequest>> GetAllWalletsRequests(long brandId)
        => Context.WalletsRequests.Where(x => x.Type == nameof(WalletRequestType.withdrawal_request) && x.Status == 0 && x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt).AsNoTracking()
            .ToListAsync();

    public Task<List<WalletsRequest>> GetAllWalletRequestRevertTransaction(long brandId)
        => Context.WalletsRequests.Where(x => x.Type == WalletRequestType.revert_invoice_request.ToString() && x.BrandId == brandId)
            .AsNoTracking().ToListAsync();

    public Task<List<WalletsRequest>> GetAllWalletRequestByAffiliateId(int id)
        => Context.WalletsRequests.Where(x => x.AffiliateId == id).AsNoTracking().ToListAsync();

    public async Task<decimal> GetTotalWalletRequestAmountByAffiliateId(int id, long brandId)
    {
        var walletRequests = await Context.WalletsRequests
            .Where(x => x.AffiliateId == id && x.Status == 0 && x.Type == WalletRequestType.withdrawal_request.ToString() && x.BrandId == brandId)
            .ToListAsync();

        decimal totalAmount = walletRequests.Sum(x => x.Amount);
        return totalAmount;
    }

    public async Task<decimal> GetTotalWalletRequestAmount(long brandId)
    {
        var walletRequests = await Context.WalletsRequests
            .Where(x => x.Status == 0 && x.Type == WalletRequestType.withdrawal_request.ToString() && x.BrandId == brandId)
            .ToListAsync();

        decimal totalAmount = walletRequests.Sum(x => x.Amount);
        return totalAmount;
    }

    public Task<List<WalletsRequest>> GetWalletRequestsByIds(List<long> ids)
        => Context.WalletsRequests.Where(x => ids.Contains(x.Id)).ToListAsync();

    public Task<WalletsRequest?> GetWalletRequestsByInvoiceNumber(int id)
        => Context.WalletsRequests.FirstOrDefaultAsync(x => x.InvoiceNumber == id);

    public async Task<WalletsRequest?> CreateWalletRequestAsync(WalletsRequest request)
    {
        FormattableString sql = $@"
        SELECT * 
          FROM wallet_service.create_wallet_request(
              {request.AffiliateId},
              {request.PaymentAffiliateId}::integer,
              {request.OrderNumber},
              {request.Amount},
              {request.Concept},
              {request.Status},
              {request.AttentionDate}::timestamp with time zone,
              {request.AdminUserName},
              {request.Type},
              {request.InvoiceNumber}::integer,
              {request.BrandId}
          )";

        var row = await Context.WalletsRequests
            .FromSqlInterpolated(sql)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        return row;
    }

    public async Task UpdateBulkWalletRequestsAsync(List<WalletsRequest> requests)
    {
        const int take = 1000;
        var packageCount = requests.Count / take;

        for (var i = 0; i <= packageCount; i++)
        {
            var packageList = requests.Skip(i * take).Take(take).ToList();
            Context.WalletsRequests.UpdateRange(packageList);
        }

        await Context.SaveChangesAsync();
    }

    public async Task<WalletsRequest> UpdateWalletRequestsAsync(WalletsRequest requests)
    {
        var today = DateTime.Now;
        requests.CreatedAt = DateTime.SpecifyKind(today, DateTimeKind.Utc);
        requests.AttentionDate = DateTime.SpecifyKind(today, DateTimeKind.Utc);
        requests.CreationDate = DateTime.SpecifyKind(today, DateTimeKind.Utc);
        requests.UpdatedAt = DateTime.SpecifyKind(today, DateTimeKind.Unspecified);
        Context.WalletsRequests.Update(requests);
        await Context.SaveChangesAsync();

        return requests;
    }


    public async Task<WalletsRequest> DeleteWalletRequestAsync(WalletsRequest request)
    {
        request.DeletedAt = DateTime.Now;

        Context.WalletsRequests.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public async Task<decimal?> GetTotalWithdrawnByAffiliateId(long affiliateId)
        => await Context.WalletsRequests
            .Where(x => x.AffiliateId == affiliateId &&
                        x.Type == "withdrawal_request" &&
                        x.Status == 1)
            .SumAsync(x => (decimal?)x.Amount);

    public async Task<WalletsRequest?> GetByIdAsync(int id)
        => await Context.WalletsRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<(bool Success, int ProcessedCount, string? ErrorMessage)> ProcessAdministrativePaymentAsync(List<WalletPaymentRequest> requests,
        long brandId, string adminUserName)
    {
        var requestsJson = JsonConvert.SerializeObject(requests.Select(r => new
        {
            id = r.Id,
            affiliate_id = r.AffiliateId,
            affiliate_user_name = r.AffiliateUserName,
            amount = r.Amount
        }));
        
        FormattableString sql = $@"
        SELECT * FROM wallet_service.process_administrative_payment(
             {requestsJson}::jsonb,
             {adminUserName},
             {Constants.AdminUserId},
             {brandId}
        )";
        
        var result = await Context.Set<PaymentProcessResult>()
            .FromSqlInterpolated(sql) 
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return result != null
            ? (result.Success, result.ProcessedCount, result.ErrorMessage)
            : (false, 0, "No se obtuvo respuesta de la base de datos");
    }
}