using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

/// <param name="Signature">HMAC sent by the provider, when the signature header is present.</param>
public record ProcessCoinPayWebhookCommand(WebhookNotificationRequest Request, string? Signature = null)
    : IRequest<bool>;
