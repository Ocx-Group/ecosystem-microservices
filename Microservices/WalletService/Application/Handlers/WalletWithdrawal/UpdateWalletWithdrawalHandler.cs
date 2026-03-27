using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWithdrawal;

public class UpdateWalletWithdrawalHandler : IRequestHandler<UpdateWalletWithdrawalCommand, WalletWithDrawalDto?>
{
    private readonly IWalletWithDrawalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWalletWithdrawalHandler> _logger;

    public UpdateWalletWithdrawalHandler(
        IWalletWithDrawalRepository repository,
        IMapper mapper,
        ILogger<UpdateWalletWithdrawalHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWithDrawalDto?> Handle(UpdateWalletWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletWithdrawalById(request.Id);

        if (entity is null)
            return null;

        entity.AffiliateId = request.AffiliateId;
        entity.Amount = request.Amount;
        entity.Observation = request.Observation;
        entity.AdminObservation = request.AdminObservation;
        entity.Date = request.Date;
        entity.ResponseDate = request.ResponseDate;
        entity.RetentionPercentage = request.RetentionPercentage;
        entity.Status = request.Status;

        var updated = await _repository.UpdateWalletWithdrawalAsync(entity);
        return _mapper.Map<WalletWithDrawalDto>(updated);
    }
}
