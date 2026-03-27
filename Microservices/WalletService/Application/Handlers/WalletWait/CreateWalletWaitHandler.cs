using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWait;

public class CreateWalletWaitHandler : IRequestHandler<CreateWalletWaitCommand, WalletWaitDto>
{
    private readonly IWalletWaitRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWalletWaitHandler> _logger;

    public CreateWalletWaitHandler(
        IWalletWaitRepository repository,
        IMapper mapper,
        ILogger<CreateWalletWaitHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWaitDto> Handle(CreateWalletWaitCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<WalletsWait>(request);
        var created = await _repository.CreateWalletWaitAsync(entity);
        return _mapper.Map<WalletWaitDto>(created);
    }
}
