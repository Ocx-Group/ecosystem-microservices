using Ecosystem.AccountService.Application.DTOs.Privilege;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Privilege;

public record UpdatePrivilegeCommand(
    int Id,
    bool CanCreate,
    bool CanRead,
    bool CanDelete,
    bool CanEdit
) : IRequest<PrivilegesDto?>;
