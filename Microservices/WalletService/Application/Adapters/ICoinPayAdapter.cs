using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface ICoinPayAdapter
{
    Task<CreateTransactionResponse?> CreateTransaction(CreateTransactionRequest request);
    Task<CreateChannelResponse?> CreateChannel(CreateChannelRequest request);
    Task<GetNetworkResponse?> GetNetworksByIdCurrency(int currencyId);
    Task<CreateAddressResponse?> CreateAddress(CreateAddresRequest request);
    Task<GetTransactionByIdResponse?> GetTransactionById(int transactionId);
    Task<SendFundsResponse?> SendFunds(SendFundRequest request);
    Task<GetTransactionByIdResponse?> GetTransactionByReference(string reference);
}
