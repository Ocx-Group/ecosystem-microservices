using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Application.Queries.Privilege;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Privilege;

public class GetPrivilegesByRolIdHandler : IRequestHandler<GetPrivilegesByRolIdQuery, ICollection<PrivilegeMenuConfigurationDto>>
{
    private readonly IPrivilegeRepository _privilegeRepo;
    private readonly IMenuConfigurationRepository _menuConfigRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPrivilegesByRolIdHandler> _logger;

    public GetPrivilegesByRolIdHandler(
        IPrivilegeRepository privilegeRepo,
        IMenuConfigurationRepository menuConfigRepo,
        IMapper mapper,
        ILogger<GetPrivilegesByRolIdHandler> logger)
    {
        _privilegeRepo = privilegeRepo;
        _menuConfigRepo = menuConfigRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<PrivilegeMenuConfigurationDto>> Handle(GetPrivilegesByRolIdQuery request, CancellationToken cancellationToken)
    {
        var menuConfigurations = await _menuConfigRepo.GetAllMenuConfigurationsAsync();
        var privileges = await _privilegeRepo.GetAllPrivileges(request.RolId);

        var result = new List<PrivilegeMenuConfigurationDto>();

        foreach (var mc in menuConfigurations)
        {
            var dto = _mapper.Map<PrivilegeMenuConfigurationDto>(mc);
            var privilege = privileges.FirstOrDefault(p => p.MenuConfigurationId == mc.Id);

            if (privilege is not null)
            {
                dto.PrivilegeId = privilege.Id;
                dto.CanCreate = privilege.CanCreate;
                dto.CanEdit = privilege.CanEdit;
                dto.CanRead = privilege.CanRead;
                dto.CanDelete = privilege.CanDelete;
            }

            result.Add(dto);
        }

        return result;
    }
}
