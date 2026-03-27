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
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Extensions;
using static Ecosystem.WalletService.Domain.Extensions.CommonExtensions;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletRepository : BaseRepository, IWalletRepository
{
    private readonly ApplicationConfiguration _appSettings;

    public WalletRepository(IOptions<ApplicationConfiguration> appSettings, WalletServiceDbContext context) :
        base(context)
        => _appSettings = appSettings.Value;

    public Task<List<Wallet>> GetWalletByAffiliateId(int affiliateId, long brandId)
        => Context.Wallets
            .Where(x => x.AffiliateId == affiliateId && x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<decimal> GetAvailableBalanceByAffiliateId(int affiliateId, long brandId)
        => await Context.Wallets
            .Where(x => x.AffiliateId == affiliateId && x.Status == true && x.BrandId == brandId)
            .SumAsync(x => (x.Credit ?? 0m) - (x.Debit ?? 0m));


    public async Task<IEnumerable<AffiliateBalance>> GetAllAffiliatesWithPositiveBalance(long brandId)
    {
        var list = await Context.Wallets
            .Where(x => x.Status == true && x.BrandId == brandId)
            .GroupBy(x => x.AffiliateId)
            .Select(g => new AffiliateBalance
            {
                AffiliateId = g.Key,
                AffiliateUserName = g.FirstOrDefault()!.AffiliateUserName,
                Balance = g.Sum(x => x.Credit - x.Debit)
            })
            .Where(ab => ab.Balance > 0)
            .ToListAsync();

        return list;
    }

    public async Task<decimal> GetAvailableBalanceAdmin(long brandId)
    {
        var result = await Context.Wallets
            .Where(x => x.Status == true && x.BrandId == brandId)
            .Select(x => x.Credit - x.Debit).SumAsync();

        return (decimal)result!;
    }

    public async Task<decimal> GetCommissionsForAdminAsync(long brandId)
    {
        var result = await Context.Wallets
            .Where(x => x.Status == true &&
                        x.BrandId == brandId &&
                        x.AffiliateId == 0 &&
                        x.ConceptType != null &&
                        x.ConceptType.Contains("commission_passed_wallet"))
            .Select(x => (decimal)(x.Credit - x.Debit)!)
            .SumAsync();

        return result;
    }

    public async Task<decimal?> GetReverseBalanceByAffiliateId(int affiliateId, long brandId)
    {
        var totalCredits = await Context.Wallets
            .Where(x => x.AffiliateId == affiliateId && x.ConceptType == WalletConceptType.revert_pool.ToString() &&
                        x.BrandId == brandId)
            .Select(x => x.Credit)
            .SumAsync();

        var totalDebits = await Context.Wallets
            .Where(x => x.AffiliateId == affiliateId &&
                        x.ConceptType == WalletConceptType.purchase_with_reverse_balance.ToString() &&
                        x.BrandId == brandId)
            .Select(x => x.Debit)
            .SumAsync();

        var reverseBalance = totalCredits - totalDebits;
        return Convert.ToDecimal(reverseBalance);
    }

    public async Task<decimal> GetTotalReverseBalance(long brandId)
    {
        var totalCredits = await Context.Wallets
            .Where(x => x.ConceptType == WalletConceptType.revert_pool.ToString() && x.BrandId == brandId)
            .Select(x => x.Credit)
            .SumAsync();

        var totalDebits = await Context.Wallets
            .Where(x =>
                x.ConceptType == WalletConceptType.purchase_with_reverse_balance.ToString() && x.BrandId == brandId)
            .Select(x => x.Debit)
            .SumAsync();

        var reverseBalance = totalCredits - totalDebits;
        return Convert.ToDecimal(reverseBalance);
    }

    public Task<decimal?> GetTotalAcquisitionsByAffiliateId(int affiliateId, long brandId)
    {
        var allowedPaymentGroups = new[] { 2, 11, 12, 13 };

        return Context.InvoicesDetails.Include(x => x.Invoice).AsNoTracking()
            .Where(x
                => x.Invoice.AffiliateId == affiliateId
                   && allowedPaymentGroups.Contains(x.PaymentGroupId)
                   && x.ProductPack
                   && !x.Invoice.CancellationDate.HasValue
                   && x.BrandId == brandId)
            .SumAsync(s => s.BaseAmount);
    }

    public async Task<decimal?> GetTotalCommissionsPaid(int affiliateId, long brandId)
    {
        var status = new HashSet<string>
        {
            "commission_passed_wallet",
            "purchasing_pool",
            "pool_commission",
            "model_four_payment",
            "model_five_payment",
            "model_six_payment",
            "membership_bonus"
        };
        var total = await Context.Wallets
            .Where(x => x.AffiliateId == affiliateId && x.ConceptType != null && status.Contains(x.ConceptType) &&
                        x.Status == true && x.BrandId == brandId)
            .SumAsync(x => x.Credit);

        return total;
    }

    public async Task<InvoicesSpResponse?> DebitTransaction(DebitTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var parameters = CreateDebitListParameters(request);
            const string sql = @"
            SELECT * FROM wallet_service.debit_transaction(
            @p_user_id, @p_concept, @p_points, @p_commissionable, @p_payment_method,
            @p_origin, @p_level, @p_debit, @p_affiliate_user_name, @p_admin_user_name,
            @p_type, @p_concept_type, @p_invoices_details, @p_brand_id,
            @p_affiliate_id, @p_bank, @p_receipt_number, @p_secret_key, @p_reason
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

    public async Task<InvoicesSpResponse?> DebitEcoPoolTransactionSp(DebitTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.DebitEcoPoolTransactionSp, sql);

            CreateDebitEcoPoolListParameters(request, cmd);

            await sql.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();
            var dd = oReader.ToDynamicList();
            var jsonString = dd.FirstOrDefault()!.ToJsonString();
            var response = jsonString.ToJsonObject<InvoicesSpResponse>();

            await sql.CloseAsync();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<InvoicesSpResponse?> AdminDebitTransaction(DebitTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var invoicesJson = JsonConvert.SerializeObject(request.invoices);

            return await Context.InvoicesSpResponses
                .FromSqlInterpolated($@"
                SELECT * 
                  FROM wallet_service.admin_debit_transaction_sp(
                      {request.AffiliateId},
                      {request.UserId},
                      {request.Concept},
                      {request.Points},
                      {request.Commissionable},
                      {request.PaymentMethod},
                      {request.Origin},
                      {request.Level},
                      {request.Debit},
                      {request.AffiliateUserName},
                      {request.AdminUserName},
                      {request.ConceptType},
                      {invoicesJson}::jsonb,
                      {request.BrandId}::integer,
                      {request.Type},
                      {request.Bank},
                      {request.ReceiptNumber},
                      {request.SecretKey},
                      {request.Reason},
                      {request.IncludeInCommissionCalculation}
                  )")
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error processing admin debit transaction", ex);
        }
    }
    
    public async Task<bool> CreditTransaction(CreditTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.CreditTransactionSp, sql);

            CreateCreditListParameters(request, cmd);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> CreditModel1ATransaction(CreditTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.CreditTransactionSpModel1A, sql);

            CreateCreditListParameters(request, cmd);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> CreditModel1BTransaction(CreditTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.CreditTransactionSpModel1B, sql);

            CreateCreditListParameters(request, cmd);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await sql.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> CreateModel2Sp(Model2TransactionRequest request)
    {
        try
        {
            await using var sqlConnection = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);

            await using var cmd = new SqlCommand(Constants.Model2RequestSp, sqlConnection);

            var levelTypeDataTable = ConvertToDataTable(request.LevelsType);
            var ecoPoolTypeDataTable = ConvertToDataTable(request.EcoPoolsType);

            CreateModel2Parameters(request, levelTypeDataTable, ecoPoolTypeDataTable, cmd);

            await sqlConnection.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();

            await sqlConnection.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateModel1ASp(Model1ATransactionRequest request)
    {
        try
        {
            await using var sqlConnection = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);

            await using var cmd = new SqlCommand(Constants.Model1ARequestSp, sqlConnection);

            var levelTypeDataTable = ConvertToDataTable(request.LevelsType);
            var ecoPoolTypeDataTable = ConvertToDataTable(request.EcoPoolsType);

            CreateModel1AParameters(request, levelTypeDataTable, ecoPoolTypeDataTable, cmd);

            await sqlConnection.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();

            await sqlConnection.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateModel1BSp(Model1BTransactionRequest request)
    {
        try
        {
            await using var sqlConnection = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);

            await using var cmd = new SqlCommand(Constants.Model1BRequestSp, sqlConnection);

            var levelTypeDataTable = ConvertToDataTable(request.LevelsType);
            var ecoPoolTypeDataTable = ConvertToDataTable(request.EcoPoolsType);

            CreateModel1BParameters(request, levelTypeDataTable, ecoPoolTypeDataTable, cmd);

            await sqlConnection.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();

            await sqlConnection.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreateModel3Sp(Model3TransactionRequest request)
    {
        try
        {
            await using var sqlConnection = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);

            await using var cmd = new SqlCommand(Constants.Model3RequestSp, sqlConnection);

            var levelTypeDataTable = ConvertToDataTable(request.LevelsType);
            var ecoPoolTypeDataTable = ConvertToDataTable(request.EcoPoolsType);

            CreateModel3Parameters(request, levelTypeDataTable, ecoPoolTypeDataTable, cmd);

            await sqlConnection.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();

            await sqlConnection.CloseAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void CreateDebitEcoPoolListParameters(DebitTransactionRequest request, SqlCommand cmd)
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
    }

    private static List<NpgsqlParameter> CreateDebitListParameters(DebitTransactionRequest request)
    {
        var invoicesDetailsJson = JsonConvert.SerializeObject(request.invoices);

        return new List<NpgsqlParameter>
        {
            new("p_user_id", NpgsqlDbType.Integer) { Value = request.UserId },
            new("p_concept", NpgsqlDbType.Text) { Value = request.Concept },
            new("p_points", NpgsqlDbType.Numeric) { Value = request.Points },
            new("p_commissionable", NpgsqlDbType.Numeric) { Value = request.Commissionable },
            new("p_payment_method", NpgsqlDbType.Text) { Value = request.PaymentMethod },
            new("p_origin", NpgsqlDbType.Smallint) { Value = request.Origin },
            new("p_level", NpgsqlDbType.Integer) { Value = request.Level },
            new("p_debit", NpgsqlDbType.Numeric) { Value = request.Debit },
            new("p_affiliate_user_name", NpgsqlDbType.Text) { Value = request.AffiliateUserName },
            new("p_admin_user_name", NpgsqlDbType.Text) { Value = request.AdminUserName },
            new("p_type", NpgsqlDbType.Boolean) { Value = request.Type },
            new("p_concept_type", NpgsqlDbType.Text) { Value = request.ConceptType },
            new("p_invoices_details", NpgsqlDbType.Jsonb) { Value = invoicesDetailsJson },
            new("p_brand_id", NpgsqlDbType.Integer) { Value = request.BrandId },
            new("p_affiliate_id", NpgsqlDbType.Integer) { Value = request.AffiliateId },
            new("p_bank", NpgsqlDbType.Text) { Value = request.Bank ?? string.Empty },
            new("p_receipt_number", NpgsqlDbType.Text) { Value = request.ReceiptNumber ?? string.Empty },
            new("p_secret_key", NpgsqlDbType.Text) { Value = request.SecretKey ?? string.Empty },
            new("p_reason", NpgsqlDbType.Text) { Value = request.Reason ?? string.Empty }
        };
    }

    private void CreateModel2Parameters(Model2TransactionRequest request, DataTable levels, DataTable ecoPools,
        SqlCommand cmd)
    {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@EcoPoolConfigurationId", SqlDbType.Int)
        {
            Value = request.EcoPoolConfigurationId
        });

        cmd.Parameters.Add(new SqlParameter("@TotalPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.TotalPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@EcoPoolPercentage", SqlDbType.Decimal)
        {
            Value = request.EcoPoolPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentage", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@Case", SqlDbType.Int)
        {
            Value = request.Case
        });

        cmd.Parameters.Add(new SqlParameter("@Points", SqlDbType.Int)
        {
            Value = request.Points
        });

        cmd.Parameters.Add(new SqlParameter("@Levels", SqlDbType.Structured)
        {
            Value = levels,
            TypeName = "dbo.LevelsType"
        });

        cmd.Parameters.Add(new SqlParameter("@Pools", SqlDbType.Structured)
        {
            Value = ecoPools,
            TypeName = "dbo.EcoPoolType"
        });
    }

    private void CreateModel1AParameters(Model1ATransactionRequest request, DataTable levels, DataTable ecoPools,
        SqlCommand cmd)
    {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@EcoPoolConfigurationId", SqlDbType.Int)
        {
            Value = request.EcoPoolConfigurationId
        });

        cmd.Parameters.Add(new SqlParameter("@TotalPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.TotalPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@EcoPoolPercentage", SqlDbType.Decimal)
        {
            Value = request.EcoPoolPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentage", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@Case", SqlDbType.Int)
        {
            Value = request.Case
        });

        cmd.Parameters.Add(new SqlParameter("@Points", SqlDbType.Int)
        {
            Value = request.Points
        });

        cmd.Parameters.Add(new SqlParameter("@Levels", SqlDbType.Structured)
        {
            Value = levels,
            TypeName = "dbo.LevelsType"
        });

        cmd.Parameters.Add(new SqlParameter("@Pools", SqlDbType.Structured)
        {
            Value = ecoPools,
            TypeName = "dbo.EcoPoolType"
        });
    }

    private void CreateModel1BParameters(Model1BTransactionRequest request, DataTable levels, DataTable ecoPools,
        SqlCommand cmd)
    {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@EcoPoolConfigurationId", SqlDbType.Int)
        {
            Value = request.EcoPoolConfigurationId
        });

        cmd.Parameters.Add(new SqlParameter("@TotalPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.TotalPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@EcoPoolPercentage", SqlDbType.Decimal)
        {
            Value = request.EcoPoolPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentage", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentage
        });

        cmd.Parameters.Add(new SqlParameter("@CompanyPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.CompanyPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@Case", SqlDbType.Int)
        {
            Value = request.Case
        });

        cmd.Parameters.Add(new SqlParameter("@Points", SqlDbType.Int)
        {
            Value = request.Points
        });

        cmd.Parameters.Add(new SqlParameter("@Levels", SqlDbType.Structured)
        {
            Value = levels,
            TypeName = "dbo.LevelsType"
        });

        cmd.Parameters.Add(new SqlParameter("@Pools", SqlDbType.Structured)
        {
            Value = ecoPools,
            TypeName = "dbo.EcoPoolType"
        });
    }

    private void CreateModel3Parameters(Model3TransactionRequest request, DataTable levels, DataTable ecoPools,
        SqlCommand cmd)
    {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("@Percentage", SqlDbType.Int)
        {
            Value = request.Percentage
        });

        cmd.Parameters.Add(new SqlParameter("@EcoPoolConfigurationId", SqlDbType.Int)
        {
            Value = request.EcoPoolConfigurationId
        });

        cmd.Parameters.Add(new SqlParameter("@TotalPercentageLevels", SqlDbType.Decimal)
        {
            Value = request.TotalPercentageLevels
        });

        cmd.Parameters.Add(new SqlParameter("@Levels", SqlDbType.Structured)
        {
            Value = levels,
            TypeName = "dbo.LevelsType"
        });

        cmd.Parameters.Add(new SqlParameter("@ItemsModelTwo", SqlDbType.Structured)
        {
            Value = ecoPools,
            TypeName = "dbo.EcoPoolType"
        });
    }

    private void CreateCreditListParameters(CreditTransactionRequest request, SqlCommand cmd)
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

        cmd.Parameters.Add(new SqlParameter("@Credit", SqlDbType.Decimal)
        {
            Value = request.Credit
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

        cmd.Parameters.Add(new SqlParameter("@ConceptType", SqlDbType.VarChar)
        {
            Value = request.ConceptType,
            Size = 50
        });

        cmd.Parameters.Add(new SqlParameter("@BrandId", SqlDbType.Int)
        {
            Value = request.BrandId
        });
    }

    public Task<List<Wallet>> GetWalletByUserId(int userId, long brandId)
        => Context.Wallets.Where(x => x.UserId == userId && x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public Task<List<Wallet>> GetWalletsRequest(int userId, long brandId)
        => Context.Wallets.AsNoTracking().Where(x
            => x.ConceptType == WalletConceptType.wallet_withdrawal_request.ToString()
               && x.AffiliateId == userId && x.BrandId == brandId).ToListAsync();

    public Task<List<Wallet>> GetAllWallets(long brandId)
        => Context.Wallets.Where(x => x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt).AsNoTracking().ToListAsync();

    public Task<List<ModelFourStatistic>> GetUserModelFour(int[] affiliateIds)
    {
        return Context.ModelFourStatistics.AsNoTracking().Where(x
            => affiliateIds.Contains(x.AffiliateId)).ToListAsync();
    }

    public Task<Wallet?> GetWalletById(int id, long brandId)
        => Context.Wallets.FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public async Task<Wallet> CreateWalletAsync(Wallet request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;

        await Context.AddAsync(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public async Task<Wallet> CreateAsync(Wallet request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;

        await Context.AddAsync(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public async Task<Wallet> UpdateWalletAsync(Wallet request)
    {
        var today = DateTime.Now;
        request.UpdatedAt = today;
        Context.Wallets.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public async Task<Wallet> DeleteWalletAsync(Wallet request)
    {
        request.DeletedAt = DateTime.Now;

        Context.Wallets.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }

    public Task<List<InvoicesDetail>> GetDebitsModel2WithinMonth(DateTime from, DateTime to)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.ProductPack && x.PaymentGroupId == 2 && x.Invoice.Status == true &&
                        x.Invoice.CancellationDate == null &&
                        x.Date >= from && x.Date <= to).ToListAsync();

    public Task<List<InvoicesDetail>> GetDebitsModel1AWithinMonth(DateTime from, DateTime to)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.PaymentGroupId == 7 && x.Invoice.Status == true && x.Invoice.CancellationDate == null &&
                        x.Date >= from && x.Date <= to).ToListAsync();

    public Task<List<InvoicesDetail>> GetDebitsModel1BWithinMonth(DateTime from, DateTime to)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.PaymentGroupId == 8 && x.Invoice.Status == true && x.Invoice.CancellationDate == null &&
                        x.Date >= from && x.Date <= to).ToListAsync();

    public Task<List<InvoicesDetail>> GetDebitsModel2OutsideMonth(DateTime date)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.Date.Year >= 2024 && x.ProductPack && x.PaymentGroupId == 2 && x.Date < date &&
                        x.Invoice.Status == true &&
                        x.Invoice.CancellationDate == null).ToListAsync();

    public Task<List<InvoicesDetail>> GetDebitsModel1AOutsideMonth(DateTime date)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.PaymentGroupId == 7 && x.Date < date && x.Invoice.Status == true &&
                        x.Invoice.CancellationDate == null).ToListAsync();

    public Task<List<InvoicesDetail>> GetDebitsModel1BOutsideMonth(DateTime date)
        => Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x => x.PaymentGroupId == 8 && x.Date < date && x.Invoice.Status == true &&
                        x.Invoice.CancellationDate == null).ToListAsync();


    public Task<List<InvoicesDetail>> GetInvoicesDetailsItemsForModel3(DateTime from, DateTime to)
    {
        return Context.InvoicesDetails.Include(i => i.Invoice).AsNoTracking()
            .Where(x
                => (x.PaymentGroupId == 6 || x.PaymentGroupId == 5) &&
                   x.Date >= from && x.Date <= to && x.Invoice.Status && x.Invoice.CancellationDate == null)
            .ToListAsync();
    }

    public async Task<bool> CreateTransferBalance(Wallet debitTransaction, Wallet creditTransaction)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        var today = DateTime.Now;
        try
        {
            debitTransaction.CreatedAt = today;
            debitTransaction.UpdatedAt = today;
            creditTransaction.CreatedAt = today;
            creditTransaction.UpdatedAt = today;
            Context.Wallets.Add(debitTransaction);
            Context.Wallets.Add(creditTransaction);

            await Context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task RevertDebitTransactionAsync(int id)
    {
        await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
        await using var cmd = new SqlCommand(Constants.RevertDebitTransaction, sql);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@InvoiceNumber", SqlDbType.Int)
        {
            Value = id
        });

        await sql.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        await sql.CloseAsync();
    }

    public async Task<bool> IsActivePoolGreaterThanOrEqualTo25(int affiliateId, long brandId)
    {
        var result = await Context.InvoicesDetails.Include(x => x.Invoice)
            .Where(x => x.ProductPack && x.Invoice.AffiliateId == affiliateId && x.Invoice.Status == true &&
                        x.Invoice.CancellationDate == null && x.PaymentGroupId == 2 && x.BrandId == brandId)
            .ToListAsync();

        if (result is { Count: 0 })
            return false;

        return result.Sum(x => x.BaseAmount) >= 25;
    }

    public async Task<InvoicesSpResponse?> HandleMembershipTransaction(DebitTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var parameters = CreateDebitListParameters(request);
            const string sql = @"
            SELECT * FROM wallet_service.handle_membership_transaction(
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
            throw new Exception("Error processing membership transaction", ex);
        }
    }

    public async Task<InvoicesSpResponse?> MembershipDebitTransaction(DebitTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var parameters = CreateDebitListParameters(request);
            const string sql = @"
            SELECT * FROM wallet_service.pay_membership_with_internal_balance(
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
            throw new Exception("Error processing pay membership with internal balance", ex);
        }
    }

    private DataTable ConvertToWalletsDataTable(Wallet[] requests)
    {
        var today = DateTime.Now;
        var dataTable = new DataTable();
        dataTable.Columns.Add("AffiliateId", typeof(int));
        dataTable.Columns.Add("UserId", typeof(int));
        dataTable.Columns.Add("Credit", typeof(decimal));
        dataTable.Columns.Add("Debit", typeof(decimal));
        dataTable.Columns.Add("Deferred", typeof(decimal));
        dataTable.Columns.Add("Status", typeof(bool));
        dataTable.Columns.Add("Concept", typeof(string));
        dataTable.Columns.Add("Support", typeof(int));
        dataTable.Columns.Add("Date", typeof(DateTime));
        dataTable.Columns.Add("Compression", typeof(bool));
        dataTable.Columns.Add("Detail", typeof(string));
        dataTable.Columns.Add("CreatedAt", typeof(DateTime));
        dataTable.Columns.Add("UpdatedAt", typeof(DateTime));
        dataTable.Columns.Add("DeletedAt", typeof(DateTime));
        dataTable.Columns.Add("AffiliateUserName", typeof(string));
        dataTable.Columns.Add("AdminUserName", typeof(string));
        dataTable.Columns.Add("ConceptType", typeof(string));

        foreach (var request in requests)
        {
            dataTable.Rows.Add(request.AffiliateId, request.UserId, 0, request.Debit, 0, true,
                request.Concept, null, today, false, "", today,
                today, null, request.AffiliateUserName, request.AdminUserName,
                request.ConceptType);
        }

        return dataTable;
    }

    public async Task<bool> BulkAdministrativeDebitTransaction(Wallet[] requests)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.BulkAdministrativeDebitSp, sql);

            var walletsDataTable = ConvertToWalletsDataTable(requests);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@WalletsData", SqlDbType.Structured)
            {
                Value = walletsDataTable,
                TypeName = "dbo.WalletType"
            });

            SqlParameter successParameter = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(successParameter);

            await sql.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            var isSuccess = (bool)successParameter.Value;

            await sql.CloseAsync();

            return isSuccess;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task TransactionPoints(int affiliateId, decimal debitLeft, decimal debitRight, decimal creditLeft,
        decimal creditRight)
    {
        // return Task.CompletedTask;
        var debit = new ModelFourStatistic()
        {
            AffiliateId = affiliateId,
            Concept = string.Empty,
            AffiliateNetworkId = affiliateId,
            DebitLeft = debitLeft,
            DebitRight = debitRight,
            CreditLeft = creditLeft,
            CreditRight = creditRight,
            Compression = false,
            Date = DateTime.Now,
            InvoiceId = 0
        };
        Context.ModelFourStatistics.Add(debit);
        return Context.SaveChangesAsync();
    }

    public Task<decimal?> GetTotalServiceBalance(int affiliateId, long brandId)
    {
        return Context.WalletsServiceModel2
            .Where(x => x.AffiliateId == affiliateId && x.BrandId == brandId)
            .Select(s => s.Credit - s.Debit)
            .SumAsync();
    }

    public async Task<InvoicesSpResponse?> DebitServiceBalanceTransaction(DebitTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.DebitEcoPoolTransactionServiceSpModel1B, sql);

            var invoicesDetails = CommonExtensions.ConvertToDataTable(request.invoices);

            CreateDebitServiceBalanceListParameters(request, invoicesDetails, cmd);

            await sql.OpenAsync();
            await using var oReader = await cmd.ExecuteReaderAsync();
            var dd = oReader.ToDynamicList();
            var jsonString = dd.FirstOrDefault()!.ToJsonString();
            var response = jsonString.ToJsonObject<InvoicesSpResponse>();


            await sql.CloseAsync();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<InvoicesSpResponse?> DebitServiceBalanceEcoPoolTransactionSp(DebitTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreditServiceBalanceTransaction(CreditTransactionRequest request)
    {
        throw new NotImplementedException();
    }


    private void CreateDebitServiceBalanceListParameters(DebitTransactionRequest request, DataTable dataTableDetails,
        SqlCommand cmd)
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

        cmd.Parameters.Add(new SqlParameter("@Debit", SqlDbType.Decimal)
        {
            Value = request.Debit
        });

        cmd.Parameters.Add(new SqlParameter("@AffiliateUserName", SqlDbType.VarChar)
        {
            Value = request.AffiliateUserName,
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

        cmd.Parameters.Add(new SqlParameter("@InvoicesDetails", SqlDbType.Structured)
        {
            Value = dataTableDetails,
            TypeName = "dbo.InvoicesDetailsType"
        });
    }

    public async Task<List<int>> DistributeCommissionsPerPurchaseAsync(DistributeCommissionsRequest request)
    {
        try
        {
            var beneficiaryIds = new List<int>();
            await using var sqlConnection = new NpgsqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await sqlConnection.OpenAsync();

            var query =
                "SELECT * FROM wallet_service.distribute_commissions_per_purchase(@AffiliateId, @InvoiceAmount, @BrandId, @AdminUserName, @LevelPercentages)";

            await using var cmd = new NpgsqlCommand(query, sqlConnection);

            cmd.Parameters.Add(new NpgsqlParameter("@AffiliateId", NpgsqlDbType.Integer)
            {
                Value = request.AffiliateId
            });

            cmd.Parameters.Add(new NpgsqlParameter("@InvoiceAmount", NpgsqlDbType.Numeric)
            {
                Value = request.InvoiceAmount
            });

            cmd.Parameters.Add(new NpgsqlParameter("@BrandId", NpgsqlDbType.Integer)
            {
                Value = request.BrandId
            });

            cmd.Parameters.Add(new NpgsqlParameter("@AdminUserName", NpgsqlDbType.Varchar)
            {
                Value = request.AdminUserName
            });

            var levelPercentagesParam =
                new NpgsqlParameter("@LevelPercentages", NpgsqlDbType.Array | NpgsqlDbType.Numeric)
                {
                    Value = request.LevelPercentages
                };
            cmd.Parameters.Add(levelPercentagesParam);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                beneficiaryIds.Add(reader.GetInt32(0));
            }

            await sqlConnection.CloseAsync();

            return beneficiaryIds;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error distributing commissions: {e.Message}");
            return new List<int>();
        }
    }

    public async Task<decimal> GetTotalCommissionsPaid(long brandId)
        => await Context.Wallets
            .Where(x => x.BrandId == brandId &&
                        x.ConceptType == nameof(WalletConceptType.commission_passed_wallet) && x.Status == true)
            .SumAsync(x => (decimal)x.Credit!);

    public async Task<decimal?> GetQualificationBalanceAsync(long userId, long brandId)
    {
        var qualificationConceptTypes = new[]
        {
            "commission_passed_wallet",
        };

        var balance = await Context.Wallets
            .Where(w => w.AffiliateId == userId &&
                        qualificationConceptTypes.Contains(w.ConceptType) &&
                        w.Status == true &&
                        w.Concept != "Comisión mensual pasada a la billetera" &&
                        w.BrandId == brandId)
            .GroupBy(w => w.UserId)
            .Select(g => g.Sum(w => w.Credit - w.Debit))
            .FirstOrDefaultAsync();

        return balance;
    }

    public async Task<List<long>> GetUserIdsWithCommissionsGreaterThanOrEqualTo50(long brandId)
    {
        var qualificationConceptTypes = new[]
        {
            "commission_passed_wallet",
        };

        var users = await Context.Wallets
            .Where(x => x.BrandId == brandId &&
                        qualificationConceptTypes.Contains(x.ConceptType) &&
                        x.Status == true &&
                        x.Concept != "Comisión mensual pasada a la billetera" &&
                        x.BrandId == brandId)
            .GroupBy(x => x.AffiliateId)
            .Where(g => g.Sum(x => (decimal)x.Credit!) >= 25)
            .Select(g => g.Key)
            .ToListAsync();

        return users;
    }
}