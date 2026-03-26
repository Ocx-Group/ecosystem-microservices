using Ecosystem.AccountService.Application.DTOs.Privilege;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Privilege;

public record CreatePrivilegeCommand(
    int RolId,
    int MenuConfigurationId,
    bool CanCreate,
    bool CanRead,
    bool CanDelete,
    bool CanEdit
) : IRequest<PrivilegesDto?>;
