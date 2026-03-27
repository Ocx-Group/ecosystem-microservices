using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Pagadito;

public record ProcessPagaditoWebhookCommand(Dictionary<string, string> Headers, string RequestBody) : IRequest<bool>;
