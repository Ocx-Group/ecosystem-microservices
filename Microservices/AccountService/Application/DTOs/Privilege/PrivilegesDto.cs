using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs.Privilege;

public class PrivilegesDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("rol_id")] public int RolId { get; set; }
    [JsonProperty("menu_configuration_id")] public int MenuConfigurationId { get; set; }
    [JsonProperty("can_create")] public bool CanCreate { get; set; }
    [JsonProperty("can_read")] public bool CanRead { get; set; }
    [JsonProperty("can_delete")] public bool CanDelete { get; set; }
    [JsonProperty("can_edit")] public bool CanEdit { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deleted_at")] public DateTime? DeletedAt { get; set; }
}
