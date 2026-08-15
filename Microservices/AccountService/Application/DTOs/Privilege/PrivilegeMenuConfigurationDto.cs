using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs.Privilege;

public class PrivilegeMenuConfigurationDto
{
    [JsonProperty("privilege_id")] public long? PrivilegeId { get; set; }
    [JsonProperty("menu_configuration_id")] public int? MenuConfigurationId { get; set; }
    [JsonProperty("menu_name")] public string? MenuName { get; set; }
    [JsonProperty("page_name")] public string PageName { get; set; } = string.Empty;
    [JsonProperty("can_create")] public bool CanCreate { get; set; }
    [JsonProperty("can_read")] public bool CanRead { get; set; }
    [JsonProperty("can_delete")] public bool CanDelete { get; set; }
    [JsonProperty("can_edit")] public bool CanEdit { get; set; }
}
