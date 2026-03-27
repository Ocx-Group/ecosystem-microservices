using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletModel1ARepository
{
    Task<InvoicesSpResponse?> DebitTransaction(DebitTransactionRequest request);

    Task<InvoicesSpResponse?> DebitEcoPoolTransactionSp(DebitTransactionRequest request);

    Task<bool> CreditTransaction(CreditTransactionRequest request);

    Task<decimal>  GetAvailableBalanceByAffiliateId(int  affiliateId);
    Task<decimal?> GetTotalAcquisitionsByAffiliateId(int affiliateId);
    Task<decimal?> GetReverseBalanceByAffiliateId(int    affiliateId);
    
    Task<decimal?> GetTotalServiceBalance(int         affiliateId);
    Task<decimal?> GetTotalCommissionsPaidBalance(int affiliateId);
    
    
    Task<InvoicesSpResponse?> DebitServiceBalanceTransaction(DebitTransactionRequest request);

    Task<InvoicesSpResponse?> DebitServiceBalanceEcoPoolTransactionSp(DebitTransactionRequest request);

    Task<bool> CreditServiceBalanceTransaction(CreditTransactionRequest request);

}