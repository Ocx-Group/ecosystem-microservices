using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletHistory;

public class DeleteWalletHistoryHandler : IRequestHandler<DeleteWalletHistoryCommand, WalletHistoryDto?>
{
    private readonly IWalletHistoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletHistoryHandler> _logger;

    public DeleteWalletHistoryHandler(
        IWalletHistoryRepository repository,
        IMapper mapper,
        ILogger<DeleteWalletHistoryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletHistoryDto?> Handle(DeleteWalletHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletHistoriesByIdAsync(request.Id);

        if (entity is null)
            return null;

        await _repository.DeleteWalletHistoriesAsync(entity);
        return _mapper.Map<WalletHistoryDto>(entity);
    }
}
