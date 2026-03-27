using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Invoice;

public record RevertCoinPaymentTransactionsCommand : IRequest<bool>;
