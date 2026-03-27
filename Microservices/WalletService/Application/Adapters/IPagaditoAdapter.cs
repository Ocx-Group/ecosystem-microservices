using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IPagaditoAdapter
{
    Task<PagaditoResponse?> ConnectAsync();
    Task<PagaditoResponse?> ExecuteTransaction(CreatePagaditoTransaction transaction);
}
