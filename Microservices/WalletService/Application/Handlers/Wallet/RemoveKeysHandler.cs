using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class RemoveKeysHandler : IRequestHandler<RemoveKeysCommand, Unit>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<RemoveKeysHandler> _logger;

    public RemoveKeysHandler(
        ICacheService cacheService,
        ILogger<RemoveKeysHandler> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Unit> Handle(RemoveKeysCommand command, CancellationToken cancellationToken)
    {
        await _cacheService.InvalidateBalanceAsync(command.Request.Users.Select(int.Parse).ToArray());
        return Unit.Value;
    }
}
