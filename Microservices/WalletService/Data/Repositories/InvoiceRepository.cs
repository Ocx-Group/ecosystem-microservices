using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.PaginationDto;
using Ecosystem.WalletService.Domain.Requests.PaginationRequest;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Extensions;

namespace Ecosystem.WalletService.Data.Repositories;

public class InvoiceRepository : BaseRepository, IInvoiceRepository
{
    private readonly ApplicationConfiguration _appSettings;
    
    public InvoiceRepository(IOptions<ApplicationConfiguration> appSettings, WalletServiceDbContext context) :
        base(context)
        => _appSettings = appSettings.Value;

    public Task<int> CountDetailsByPaymentGroup(int paymentGroupId, int userId, long brandId)
        => Context.InvoicesDetails.Where(x
            => !x.Invoice.CancellationDate.HasValue && x.Invoice.Status &&
               x.Invoice.AffiliateId == userId &&
               x.PaymentGroupId == paymentGroupId && x.BrandId == brandId).CountAsync();

    public Task<int> CountDetailsModel3ByPaymentGroup(int userId, long brandId)
        => Context.InvoicesDetails.Where(x
            => !x.Invoice.CancellationDate.HasValue && x.Invoice.Status &&
               x.Invoice.AffiliateId == userId &&
               new[] { 5, 6 }.Contains(x.PaymentGroupId) && x.BrandId == brandId).CountAsync();

    public Task<List<ModelFourStatistic>> Model4StatisticsByUser(int userId)
        => Context.ModelFourStatistics.Where(x => x.AffiliateId == userId).ToListAsync();

    public Task<List<Invoice>> GetAllInvoicesUser(int id, long brandId)
        => Context.Invoices.Include(x => x.InvoicesDetails).Where(x => x.AffiliateId == id && x.BrandId == brandId)
            .AsNoTracking().ToListAsync();

    public async Task<PaginationDto<Invoice>> GetAllInvoices(long brandId, PaginationRequest request)
    {
        var query = Context.Invoices
            .Include(x => x.InvoicesDetails)
            .Where(x => x.BrandId == brandId);

        if (request.StartDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= request.EndDate.Value.AddDays(1).AddTicks(-1));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAt)  
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PaginationDto<Invoice>
        {
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Items = items
        };
    }

    public Task<Invoice?> GetInvoiceById(long id, long brandId)
        => Context.Invoices.Include(e => e.InvoicesDetails)
            .FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public Task<Invoice?> GetInvoiceByReceiptNumber(string receiptNumber, long brandId)
        => Context.Invoices.Include(x => x.InvoicesDetails)
            .FirstOrDefaultAsync(e => e.ReceiptNumber == receiptNumber && e.BrandId == brandId);

    public Task<bool> InvoiceExistsByReceiptNumber(string idTransaction, long brandId)
        => Context.Invoices
            .AnyAsync(e => e.ReceiptNumber == idTransaction && e.BrandId == brandId);

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
    {
        var today = DateTime.Now;
        invoice.CreatedAt = today;
        invoice.UpdatedAt = today;

        await Context.AddAsync(invoice);
        await Context.SaveChangesAsync();

        return invoice;
    }

    public async Task<Invoice> DeleteInvoiceAsync(Invoice invoice)
    {
        var today = DateTime.Now;
        invoice.DeletedAt = today;
        invoice.UpdatedAt = today;
        invoice.CancellationDate = today;
        invoice.Status = false;

        Context.Update(invoice);
        await Context.SaveChangesAsync();

        return invoice;
    }

    public async Task<List<Invoice>> DeleteMultipleInvoicesAndDetailsAsync(long[] invoiceIds, long brandId)
    {
        var today = DateTime.Now;

        var invoices = await Context.Invoices
            .Include(i => i.InvoicesDetails)
            .Where(i => invoiceIds.Contains(i.Id) && i.BrandId == brandId)
            .ToListAsync();

        if (!invoices.Any())
        {
            return null;
        }

        foreach (var invoice in invoices)
        {
            invoice.DeletedAt = today;
            invoice.UpdatedAt = today;
            invoice.CancellationDate = today;
            invoice.Status = false;

            foreach (var detail in invoice.InvoicesDetails)
            {
                detail.DeletedAt = today;
                detail.UpdatedAt = today;
            }
        }

        await Context.SaveChangesAsync();

        return invoices;
    }

    public async Task<InvoicesSpResponse?> HandleDebitTransaction(DebitTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var parameters = CreateParameters(request);
            const string sql = @"
            SELECT * FROM wallet_service.handle_debit_transaction(
                @p_affiliate_id, @p_user_id, @p_concept, @p_points, @p_commissionable,
                @p_payment_method, @p_origin, @p_level, @p_debit, @p_affiliate_user_name,
                @p_admin_user_name, @p_concept_type, @p_invoices_details, @p_brand_id,
                @p_bank, @p_receipt_number, @p_type, @p_secret_key, @p_reason
            )";

            return await Context.InvoicesSpResponses
                .FromSqlRaw(sql, parameters.ToArray())
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error processing debit transaction", ex);
        }
    }

    private static List<NpgsqlParameter> CreateParameters(DebitTransactionRequest request)
    {
        var invoicesDetailsJson = JsonConvert.SerializeObject(request.invoices);

        return new List<NpgsqlParameter>
        {
            new("p_affiliate_id", NpgsqlDbType.Integer) { Value = request.AffiliateId },
            new("p_user_id", NpgsqlDbType.Integer) { Value = request.UserId },
            new("p_concept", NpgsqlDbType.Varchar) { Value = request.Concept },
            new("p_points", NpgsqlDbType.Numeric) { Value = request.Points },
            new("p_commissionable", NpgsqlDbType.Numeric) { Value = request.Commissionable },
            new("p_payment_method", NpgsqlDbType.Varchar) { Value = request.PaymentMethod },
            new("p_origin", NpgsqlDbType.Smallint) { Value = request.Origin },
            new("p_level", NpgsqlDbType.Integer) { Value = request.Level },
            new("p_debit", NpgsqlDbType.Numeric) { Value = request.Debit },
            new("p_affiliate_user_name", NpgsqlDbType.Varchar) { Value = request.AffiliateUserName },
            new("p_admin_user_name", NpgsqlDbType.Varchar) { Value = request.AdminUserName },
            new("p_concept_type", NpgsqlDbType.Varchar) { Value = request.ConceptType },
            new("p_invoices_details", NpgsqlDbType.Jsonb) { Value = invoicesDetailsJson },
            new("p_brand_id", NpgsqlDbType.Integer) { Value = request.BrandId },
            new("p_bank", NpgsqlDbType.Varchar) { Value = request.Bank ?? string.Empty },
            new("p_receipt_number", NpgsqlDbType.Varchar) { Value = request.ReceiptNumber ?? string.Empty },
            new("p_type", NpgsqlDbType.Boolean) { Value = request.Type },
            new("p_secret_key", NpgsqlDbType.Varchar) { Value = request.SecretKey ?? string.Empty },
            new("p_reason", NpgsqlDbType.Varchar) { Value = request.Reason ?? string.Empty }
        };
    }

    private void CreateDebitListParameters(DebitTransactionRequest request, DataTable dataTableDetails, SqlCommand cmd)
    {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@AffiliateId", SqlDbType.Int)
        {
            Value = request.AffiliateId
        });

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)
        {
            Value = request.UserId
        });

        cmd.Parameters.Add(new SqlParameter("@Concept", SqlDbType.VarChar)
        {
            Value = request.Concept,
            Size = 255
        });

        cmd.Parameters.Add(new SqlParameter("@Points", SqlDbType.Decimal)
        {
            Value = request.Points
        });

        cmd.Parameters.Add(new SqlParameter("@Commissionable", SqlDbType.Decimal)
        {
            Value = request.Commissionable
        });

        cmd.Parameters.Add(new SqlParameter("@Bank", SqlDbType.VarChar)
        {
            Value = string.IsNullOrEmpty(request.Bank) ? null : request.Bank,
            IsNullable = true,
            Size = 250
        });

        cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar)
        {
            Value = request.PaymentMethod,
            Size = 50
        });

        cmd.Parameters.Add(new SqlParameter("@Origin", SqlDbType.TinyInt)
        {
            Value = request.Origin
        });

        cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.Int)
        {
            Value = request.Level
        });

        cmd.Parameters.Add(new SqlParameter("@Debit", SqlDbType.Decimal)
        {
            Value = request.Debit
        });

        cmd.Parameters.Add(new SqlParameter("@AffiliateUserName", SqlDbType.VarChar)
        {
            Value = request.AffiliateUserName,
            Size = 50
        });

        cmd.Parameters.Add(new SqlParameter("@AdminUserName", SqlDbType.VarChar)
        {
            Value = request.AdminUserName,
            Size = 50
        });

        cmd.Parameters.Add(new SqlParameter("@ReceiptNumber", SqlDbType.VarChar)
        {
            Value = string.IsNullOrEmpty(request.ReceiptNumber) ? null : request.ReceiptNumber,
            IsNullable = true,
            Size = 100
        });

        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.Bit)
        {
            Value = request.Type
        });

        cmd.Parameters.Add(new SqlParameter("@SecretKey", SqlDbType.VarChar)
        {
            Value = string.IsNullOrEmpty(request.SecretKey) ? null : request.SecretKey,
            IsNullable = true,
            Size = 40
        });

        cmd.Parameters.Add(new SqlParameter("@ConceptType", SqlDbType.VarChar)
        {
            Value = request.ConceptType,
            Size = 50
        });

        cmd.Parameters.Add(new SqlParameter("@InvoicesDetails", SqlDbType.Structured)
        {
            Value = dataTableDetails,
            TypeName = "dbo.InvoicesDetailsTypeWithBrand"
        });

        cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar)
        {
            Value = string.IsNullOrEmpty(request.Reason) ? null : request.Reason,
            IsNullable = true,
            Size = 250
        });

        cmd.Parameters.Add(new SqlParameter("@BrandId", SqlDbType.Int)
        {
            Value = request.BrandId
        });
    }

    public async Task RevertCoinPaymentTransactions(List<InvoiceNumber> invoiceNumbers)
    {
        var table = new DataTable();
        table.Columns.Add("InvoiceNumber", typeof(int));
        foreach (var invoiceNumber in invoiceNumbers)
        {
            table.Rows.Add(invoiceNumber.InvoiceNumberValue);
        }

        var param = new SqlParameter("@InvoiceNumbers", SqlDbType.Structured)
        {
            TypeName = "dbo.InvoiceNumbersType",
            Value = table
        };

        await Context.Database.ExecuteSqlRawAsync(
            "EXEC " + Constants.CoinPaymentRevertTransactions + " @InvoiceNumbers", param);
    }

    public Task<List<Invoice>> GetInvoicesByReceiptNumber(ICollection<string> transactionIds)
        => Context.Invoices.Where(e => e.ReceiptNumber != null && transactionIds.Contains(e.ReceiptNumber))
            .ToListAsync();

    public Task<bool> GetInvoicesForTradingAcademyPurchases(int affiliateId)
        => Context.Invoices
            .Include(x => x.InvoicesDetails)
            .AnyAsync(x => x.AffiliateId == affiliateId && x.InvoicesDetails.Any(d => d.PaymentGroupId == 6));

    public async Task<List<InvoicesTradingAcademyResponse>?> GetAllInvoicesForTradingAcademyPurchases()
    {
        await using var command = Context.Database.GetDbConnection().CreateCommand();
        command.CommandText = Constants.GetTradingAcademyDetailsSp;
        command.CommandType = CommandType.StoredProcedure;

        await Context.Database.OpenConnectionAsync();

        await using var result = await command.ExecuteReaderAsync();
        var invoiceDetailsList = new List<InvoicesTradingAcademyResponse>();

        while (await result.ReadAsync())
        {
            invoiceDetailsList.Add(new InvoicesTradingAcademyResponse
            {
                ProductId = (int)result["ProductId"],
                UserName = result["UserName"].ToString() ?? "",
                InvoiceId = (int)result["InvoiceId"],
                ProductName = result["ProductName"].ToString() ?? "",
                ProductPrice = (decimal)result["ProductPrice"],
                CreatedAt = (DateTime)result["CreatedAt"]
            });
        }

        return invoiceDetailsList;
    }

    public async Task<List<InvoiceModelOneAndTwoResponse>?> GetAllInvoicesModelOneAndTwo()
    {
        await using var command = Context.Database.GetDbConnection().CreateCommand();
        command.CommandText = Constants.GetAllDetailsModelOneAndTwo;
        command.CommandType = CommandType.StoredProcedure;

        await Context.Database.OpenConnectionAsync();

        await using var result = await command.ExecuteReaderAsync();
        var invoiceDetailsList = new List<InvoiceModelOneAndTwoResponse>();

        while (await result.ReadAsync())
        {
            invoiceDetailsList.Add(new InvoiceModelOneAndTwoResponse
            {
                UserName = result["UserName"].ToString() ?? "",
                InvoiceId = (int)result["InvoiceId"],
                ProductName = result["ProductName"].ToString() ?? "",
                BaseAmount = (decimal)result["BaseAmount"],
                PaymentGroupId = (int)result["PaymentGroupId"],
                CreatedAt = (DateTime)result["CreatedAt"]
            });
        }

        return invoiceDetailsList;
    }

    public async Task<decimal> GetTotalAdquisitionsAdmin(long brandId, int paymentGroupId)
        => await Context.InvoicesDetails.Where(x =>
                !x.Invoice.CancellationDate.HasValue &&
                x.Invoice.Status &&
                x.Invoice.BrandId == brandId &&
                x.PaymentGroupId == paymentGroupId)
            .SumAsync(x => x.BaseAmount ?? 0);
    
    public async IAsyncEnumerable<List<Invoice>> GetInvoicesInBatches(DateTime? startDate, DateTime? endDate, int batchSize, long brandId)
    {
        int skip = 0;
        List<Invoice> batch;

        do
        {
            batch = await Context.Invoices
                .Include(x => x.InvoicesDetails)
                .Where(x => x.BrandId == brandId)
                .WhereIf(startDate.HasValue, x => x.CreatedAt >= startDate!.Value)
                .WhereIf(endDate.HasValue, x => x.CreatedAt <= endDate!.Value)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(batchSize)
                .ToListAsync();

            if (batch.Any())
            {
                yield return batch;
                skip += batchSize;
            }
        } while (batch.Count == batchSize);
    }
}