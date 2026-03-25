using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs.Role;

public class RoleDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deleted_at")] public DateTime? DeletedAt { get; set; }
    [JsonProperty("associated_users")] public int AssociatedUsers { get; set; }
    [JsonProperty("permissions")] public int Permissions { get; set; }
}
