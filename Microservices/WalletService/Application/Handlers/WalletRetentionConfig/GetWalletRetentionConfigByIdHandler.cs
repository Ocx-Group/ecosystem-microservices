using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletRetentionConfig;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRetentionConfig;

public class GetWalletRetentionConfigByIdHandler : IRequestHandler<GetWalletRetentionConfigByIdQuery, WalletRetentionConfigDto?>
{
    private readonly IWalletRetentionConfigRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletRetentionConfigByIdHandler> _logger;

    public GetWalletRetentionConfigByIdHandler(
        IWalletRetentionConfigRepository repository,
        IMapper mapper,
        ILogger<GetWalletRetentionConfigByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletRetentionConfigDto?> Handle(GetWalletRetentionConfigByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletRetentionConfigById(request.Id);
        return entity is null ? null : _mapper.Map<WalletRetentionConfigDto>(entity);
    }
}
