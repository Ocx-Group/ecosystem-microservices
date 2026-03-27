using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletHistory;

public class UpdateWalletHistoryHandler : IRequestHandler<UpdateWalletHistoryCommand, WalletHistoryDto?>
{
    private readonly IWalletHistoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWalletHistoryHandler> _logger;

    public UpdateWalletHistoryHandler(
        IWalletHistoryRepository repository,
        IMapper mapper,
        ILogger<UpdateWalletHistoryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletHistoryDto?> Handle(UpdateWalletHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletHistoriesByIdAsync(request.Id);

        if (entity is null)
            return null;

        entity.AffiliateId = request.AffiliateId;
        entity.UserId = request.UserId;
        entity.Credit = request.Credit;
        entity.Debit = request.Debit;
        entity.Deferred = request.Deferred;
        entity.Status = request.Status;
        entity.Concept = request.Concept;
        entity.Support = request.Support;
        entity.Date = request.Date;
        entity.UpdatedAt = DateTime.Now;

        var updated = await _repository.UpdateWalletHistoriesAsync(entity);
        return _mapper.Map<WalletHistoryDto>(updated);
    }
}
