using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface ICoinPaymentsAdapter
{
    Task<GetBasicInfoResponse?> GetProfile();
    Task<GetDepositAddressResponse?> GetDepositAddress(string currency);
    Task<GetCoinBalancesResponse?> GetCoinBalances();
    Task<CreateConPaymentsTransactionResponse?> CreatePayment(ConPaymentRequest request);
    Task<GetTransactionInfoResponse?> GetTransactionInfo(string txnId);
    Task<CoinPaymentWithdrawalResponse?> CreateMassWithdrawal(IEnumerable<CoinPaymentMassWithdrawalRequest> requests);
}
