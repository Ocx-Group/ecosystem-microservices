using Ecosystem.AccountService.Application.DTOs.Privilege;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Privilege;

public record CreatePrivilegeCommand : IRequest<PrivilegesDto?>
{
    [JsonProperty("rol_id")] public int RolId { get; init; }

    [JsonProperty("menu_configuration_id")] public int MenuConfigurationId { get; init; }

    [JsonProperty("can_create")] public bool CanCreate { get; init; }

    [JsonProperty("can_read")] public bool CanRead { get; init; }

    [JsonProperty("can_delete")] public bool CanDelete { get; init; }

    [JsonProperty("can_edit")] public bool CanEdit { get; init; }
}
