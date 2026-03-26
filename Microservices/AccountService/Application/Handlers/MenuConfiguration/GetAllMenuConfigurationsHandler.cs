using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.MenuConfiguration;
using Ecosystem.AccountService.Application.Queries.MenuConfiguration;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.MenuConfiguration;

public class GetAllMenuConfigurationsHandler : IRequestHandler<GetAllMenuConfigurationsQuery, ICollection<MenuConfigurationDto>>
{
    private readonly IMenuConfigurationRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllMenuConfigurationsHandler> _logger;

    public GetAllMenuConfigurationsHandler(
        IMenuConfigurationRepository repo,
        IMapper mapper,
        ILogger<GetAllMenuConfigurationsHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<MenuConfigurationDto>> Handle(GetAllMenuConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var menuConfigurations = await _repo.GetAllMenuConfigurationsAsync();
        return _mapper.Map<ICollection<MenuConfigurationDto>>(menuConfigurations);
    }
}
