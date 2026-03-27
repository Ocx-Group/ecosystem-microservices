using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWait;

public class UpdateWalletWaitHandler : IRequestHandler<UpdateWalletWaitCommand, WalletWaitDto?>
{
    private readonly IWalletWaitRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWalletWaitHandler> _logger;

    public UpdateWalletWaitHandler(
        IWalletWaitRepository repository,
        IMapper mapper,
        ILogger<UpdateWalletWaitHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWaitDto?> Handle(UpdateWalletWaitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletWaitById(request.Id);

        if (entity is null)
            return null;

        entity.AffiliateId = request.AffiliateId;
        entity.Credit = request.Credit;
        entity.PaymentMethod = request.PaymentMethod!;
        entity.Bank = request.Bank;
        entity.Support = request.Support;
        entity.DepositDate = request.DepositDate;
        entity.Status = request.Status;
        entity.Attended = request.Attended;
        entity.Date = request.Date;
        entity.Order = request.Order;
        entity.UpdatedAt = DateTime.Now;

        var updated = await _repository.UpdateWalletWaitAsync(entity);
        return _mapper.Map<WalletWaitDto>(updated);
    }
}
