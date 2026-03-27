using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletRequest;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class ProcessWalletRequestOptionHandler : IRequestHandler<ProcessWalletRequestOptionCommand, ServicesResponse?>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ProcessWalletRequestOptionHandler> _logger;

    public ProcessWalletRequestOptionHandler(
        IWalletRequestRepository walletRequestRepository,
        ICacheService cacheService,
        ILogger<ProcessWalletRequestOptionHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<ServicesResponse?> Handle(ProcessWalletRequestOptionCommand command, CancellationToken cancellationToken)
    {
        var response = new ServicesResponse();

        switch (command.Option)
        {
            case 1:
                await CancelWalletRequestsAsync(command.Ids);
                response.Success = true;
                response.Message = "The request has been processed correctly";
                break;
            case 2:
                response.Success = true;
                break;
            case 3:
                response.Success = true;
                break;
            case 4:
                response.Success = true;
                break;
        }

        return response;
    }

    private async Task CancelWalletRequestsAsync(List<long> ids)
    {
        var idsList = await _walletRequestRepository.GetWalletRequestsByIds(ids);

        if (idsList is { Count: 0 })
            return;

        var today = DateTime.Now;

        Parallel.ForEach(idsList, item =>
        {
            item.AttentionDate = today;
            item.UpdatedAt = today;
            item.CreatedAt = today;
            item.CreationDate = today;
            item.Status = WalletRequestStatus.cancel.ToByte();
        });

        foreach (var userId in ids)
        {
            await _cacheService.InvalidateBalanceAsync((int)userId);
        }

        await _walletRequestRepository.UpdateBulkWalletRequestsAsync(idsList.ToList());
    }
}
