using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.CoinPayments;

public record GetCoinPaymentsProfileQuery(string PbnTag) : IRequest<GetBasicInfoResponse?>;
