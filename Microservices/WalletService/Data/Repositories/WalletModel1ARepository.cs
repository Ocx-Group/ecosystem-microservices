using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Extensions;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletModel1ARepository : BaseRepository, IWalletModel1ARepository
{
    private readonly ApplicationConfiguration _appSettings;

    public WalletModel1ARepository(IOptions<ApplicationConfiguration> appSettings, WalletServiceDbContext context) :
        base(context)
        => _appSettings = appSettings.Value;


    public async Task<bool> CreditTransaction(CreditTransactionRequest request)
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


    public async Task<InvoicesSpResponse?> DebitEcoPoolTransactionSp(DebitTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.DebitEcoPoolTransactionSpModel1A, sql);

            CreateDebitEcoPoolListParameters(request, cmd);

            await sql.OpenAsync();
            await using var oReader    = await cmd.ExecuteReaderAsync();
            var             dd         = oReader.ToDynamicList();
            var             jsonString = dd.FirstOrDefault()!.ToJsonString();
            var             response   = jsonString.ToJsonObject<InvoicesSpResponse>();


            await sql.CloseAsync();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<InvoicesSpResponse?> DebitTransaction(DebitTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.DebitTransactionSpModel1A, sql);

            var invoicesDetails = CommonExtensions.ConvertToDataTable(request.invoices);

            CreateDebitListParameters(request, invoicesDetails, cmd);

            await sql.OpenAsync();
            await using var oReader    = await cmd.ExecuteReaderAsync();
            var             dd         = oReader.ToDynamicList();
            var             jsonString = dd.FirstOrDefault()!.ToJsonString();
            var             response   = jsonString.ToJsonObject<InvoicesSpResponse>();


            await sql.CloseAsync();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
            Size  = 255
        });

        cmd.Parameters.Add(new SqlParameter("@Credit", SqlDbType.Decimal)
        {
            Value = request.Credit
        });

        cmd.Parameters.Add(new SqlParameter("@AffiliateUserName", SqlDbType.VarChar)
        {
            Value = request.AffiliateUserName,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@AdminUserName", SqlDbType.VarChar)
        {
            Value = request.AdminUserName,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@ConceptType", SqlDbType.VarChar)
        {
            Value = request.ConceptType,
            Size  = 50
        });
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
            Size  = 255
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
            Value      = string.IsNullOrEmpty(request.Bank) ? null : request.Bank,
            IsNullable = true,
            Size       = 250
        });

        cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar)
        {
            Value = request.PaymentMethod,
            Size  = 50
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
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@AdminUserName", SqlDbType.VarChar)
        {
            Value = request.AdminUserName,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@ReceiptNumber", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.ReceiptNumber) ? null : request.ReceiptNumber,
            IsNullable = true,
            Size       = 100
        });

        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.Bit)
        {
            Value = request.Type
        });

        cmd.Parameters.Add(new SqlParameter("@SecretKey", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.SecretKey) ? null : request.SecretKey,
            IsNullable = true,
            Size       = 40
        });

        cmd.Parameters.Add(new SqlParameter("@ConceptType", SqlDbType.VarChar)
        {
            Value = request.ConceptType,
            Size  = 50
        });
    }
    private void CreateDebitServiceBalanceListParameters(DebitTransactionRequest request, DataTable dataTableDetails, SqlCommand cmd)
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
            Size  = 255
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
            Value      = string.IsNullOrEmpty(request.Bank) ? null : request.Bank,
            IsNullable = true,
            Size       = 250
        });

        cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar)
        {
            Value = request.PaymentMethod,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@Debit", SqlDbType.Decimal)
        {
            Value = request.Debit
        });

        cmd.Parameters.Add(new SqlParameter("@AffiliateUserName", SqlDbType.VarChar)
        {
            Value = request.AffiliateUserName,
            Size  = 50
        });
        

        cmd.Parameters.Add(new SqlParameter("@ReceiptNumber", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.ReceiptNumber) ? null : request.ReceiptNumber,
            IsNullable = true,
            Size       = 100
        });

        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.Bit)
        {
            Value = request.Type
        });

        cmd.Parameters.Add(new SqlParameter("@SecretKey", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.SecretKey) ? null : request.SecretKey,
            IsNullable = true,
            Size       = 40
        });
        
        cmd.Parameters.Add(new SqlParameter("@InvoicesDetails", SqlDbType.Structured)
        {
            Value    = dataTableDetails,
            TypeName = "dbo.InvoicesDetailsType"
        });
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
            Size  = 255
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
            Value      = string.IsNullOrEmpty(request.Bank) ? null : request.Bank,
            IsNullable = true,
            Size       = 250
        });

        cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar)
        {
            Value = request.PaymentMethod,
            Size  = 50
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
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@AdminUserName", SqlDbType.VarChar)
        {
            Value = request.AdminUserName,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@ReceiptNumber", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.ReceiptNumber) ? null : request.ReceiptNumber,
            IsNullable = true,
            Size       = 100
        });

        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.Bit)
        {
            Value = request.Type
        });

        cmd.Parameters.Add(new SqlParameter("@SecretKey", SqlDbType.VarChar)
        {
            Value      = string.IsNullOrEmpty(request.SecretKey) ? null : request.SecretKey,
            IsNullable = true,
            Size       = 40
        });

        cmd.Parameters.Add(new SqlParameter("@ConceptType", SqlDbType.VarChar)
        {
            Value = request.ConceptType,
            Size  = 50
        });

        cmd.Parameters.Add(new SqlParameter("@InvoicesDetails", SqlDbType.Structured)
        {
            Value    = dataTableDetails,
            TypeName = "dbo.InvoicesDetailsType"
        });
    }

    public async Task<decimal> GetAvailableBalanceByAffiliateId(int affiliateId)
    {
        var list = await Context.WalletsModel1A
                       .Where(x => x.AffiliateId == affiliateId && x.Status == true).ToListAsync();

        var result = list.Sum(x => x.Credit - x.Debit);
        return result.ToDecimal();
    }

    public Task<decimal?> GetTotalAcquisitionsByAffiliateId(int affiliateId)
        => Context.InvoicesDetails
            .Include(x => x.Invoice)
            .AsNoTracking()
            .Where(x => x.Invoice.AffiliateId == affiliateId && x.PaymentGroupId == 7)
            .Select(s => (decimal?)s.BaseAmount) 
            .SumAsync();

    public async Task<decimal?> GetReverseBalanceByAffiliateId(int affiliateId)
    {
        var totalCredits = await Context.WalletsModel1A
                               .Where(x => x.AffiliateId == affiliateId &&
                                           x.ConceptType == WalletConceptType.revert_pool.ToString())
                               .Select(x => x.Credit)
                               .SumAsync();

        var totalDebits = await Context.WalletsModel1A
                              .Where(x => x.AffiliateId == affiliateId &&
                                          x.ConceptType == WalletConceptType.purchase_with_reverse_balance.ToString())
                              .Select(x => x.Debit)
                              .SumAsync();

        var reverseBalance = totalCredits - totalDebits;
        return Convert.ToDecimal(reverseBalance);
    }

    public Task<decimal?> GetTotalServiceBalance(int affiliateId)
    {
        return Context.WalletsServiceModel1A
            .Where(x => x.AffiliateId == affiliateId)
            .Select(s => s.Credit - s.Debit)
            .SumAsync();
    }

    public Task<decimal?> GetTotalCommissionsPaidBalance(int affiliateId)
    {
        return Context.WalletsServiceModel1A.Where(x => x.AffiliateId == affiliateId && x.Credit.HasValue && x.Credit > 0)
            .SumAsync(s => s.Credit);
    }

    public async Task<InvoicesSpResponse?> DebitServiceBalanceTransaction(DebitTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.DebitEcoPoolTransactionServiceSpModel1A, sql);

            var invoicesDetails = CommonExtensions.ConvertToDataTable(request.invoices);

            CreateDebitServiceBalanceListParameters(request, invoicesDetails, cmd);

            await sql.OpenAsync();
            await using var oReader    = await cmd.ExecuteReaderAsync();
            var             dd         = oReader.ToDynamicList();
            var             jsonString = dd.FirstOrDefault()!.ToJsonString();
            var             response   = jsonString.ToJsonObject<InvoicesSpResponse>();


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

    public async Task<bool> CreditServiceBalanceTransaction(CreditTransactionRequest request)
    {
        try
        {
            await using var sql = new SqlConnection(_appSettings.ConnectionStrings?.PostgreSqlConnection);
            await using var cmd = new SqlCommand(Constants.CreditEcoPoolTransactionServiceSpModel1A, sql);

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
}