using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletPeriod;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class DeleteWalletPeriodHandler : IRequestHandler<DeleteWalletPeriodCommand, WalletPeriodDto?>
{
    private readonly IWalletPeriodRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletPeriodHandler> _logger;

    public DeleteWalletPeriodHandler(
        IWalletPeriodRepository repository,
        IMapper mapper,
        ILogger<DeleteWalletPeriodHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletPeriodDto?> Handle(DeleteWalletPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletPeriodById(request.Id);

        if (entity is null)
            return null;

        await _repository.DeleteWalletPeriodAsync(entity);
        return _mapper.Map<WalletPeriodDto>(entity);
    }
}
