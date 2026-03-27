using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPay;

public record GetCoinPayTransactionByReferenceQuery(string Reference) : IRequest<GetTransactionByIdResponse?>;
