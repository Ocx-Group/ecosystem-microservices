using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletRetentionConfig;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRetentionConfig;

public class DeleteWalletRetentionConfigHandler : IRequestHandler<DeleteWalletRetentionConfigCommand, WalletRetentionConfigDto?>
{
    private readonly IWalletRetentionConfigRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletRetentionConfigHandler> _logger;

    public DeleteWalletRetentionConfigHandler(
        IWalletRetentionConfigRepository repository,
        IMapper mapper,
        ILogger<DeleteWalletRetentionConfigHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletRetentionConfigDto?> Handle(DeleteWalletRetentionConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletRetentionConfigById(request.Id);

        if (entity is null)
            return null;

        await _repository.DeleteWalletRetentionConfigAsync(entity);
        return _mapper.Map<WalletRetentionConfigDto>(entity);
    }
}
