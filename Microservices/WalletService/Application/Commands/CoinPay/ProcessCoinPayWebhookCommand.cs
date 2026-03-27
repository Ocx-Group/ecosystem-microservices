using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record ProcessCoinPayWebhookCommand(WebhookNotificationRequest Request) : IRequest<bool>;
