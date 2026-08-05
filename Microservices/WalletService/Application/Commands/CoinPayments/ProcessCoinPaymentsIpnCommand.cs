using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPayments;

/// <summary>
/// An incoming CoinPayments notification. <paramref name="RawBody"/> is the untouched request
/// body: the provider's HMAC is taken over those exact bytes, so it cannot be rebuilt from
/// <paramref name="Request"/> after model binding.
/// </summary>
public record ProcessCoinPaymentsIpnCommand(
    IpnRequest Request,
    Dictionary<string, string> Headers,
    string RawBody) : IRequest<bool>;
