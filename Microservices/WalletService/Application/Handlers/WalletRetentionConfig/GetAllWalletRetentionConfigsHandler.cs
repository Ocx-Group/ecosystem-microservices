using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletRetentionConfig;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRetentionConfig;

public class GetAllWalletRetentionConfigsHandler : IRequestHandler<GetAllWalletRetentionConfigsQuery, ICollection<WalletRetentionConfigDto>>
{
    private readonly IWalletRetentionConfigRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWalletRetentionConfigsHandler> _logger;

    public GetAllWalletRetentionConfigsHandler(
        IWalletRetentionConfigRepository repository,
        IMapper mapper,
        ILogger<GetAllWalletRetentionConfigsHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<WalletRetentionConfigDto>> Handle(GetAllWalletRetentionConfigsQuery request, CancellationToken cancellationToken)
    {
        var configs = await _repository.GetAllWalletsRetentionConfig();
        return _mapper.Map<ICollection<WalletRetentionConfigDto>>(configs);
    }
}
