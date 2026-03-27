using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWait;

public class DeleteWalletWaitHandler : IRequestHandler<DeleteWalletWaitCommand, WalletWaitDto?>
{
    private readonly IWalletWaitRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletWaitHandler> _logger;

    public DeleteWalletWaitHandler(
        IWalletWaitRepository repository,
        IMapper mapper,
        ILogger<DeleteWalletWaitHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWaitDto?> Handle(DeleteWalletWaitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletWaitById(request.Id);

        if (entity is null)
            return null;

        await _repository.DeleteWalletWaitAsync(entity);
        return _mapper.Map<WalletWaitDto>(entity);
    }
}
