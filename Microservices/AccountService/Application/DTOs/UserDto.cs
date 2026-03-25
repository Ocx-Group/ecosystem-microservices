using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class UserDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("rol_id")] public int RolId { get; set; }
    [JsonProperty("is_affiliate")] public sbyte IsAffiliate { get; set; }
    [JsonProperty("rol_name")] public string RolName { get; set; } = string.Empty;
    [JsonProperty("user_name")] public string UserName { get; set; } = string.Empty;
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("last_name")] public string? LastName { get; set; }
    [JsonProperty("email")] public string Email { get; set; } = string.Empty;
    [JsonProperty("phone")] public string? Phone { get; set; }
    [JsonProperty("address")] public string? Address { get; set; }
    [JsonProperty("observation")] public string? Observation { get; set; }
    [JsonProperty("status")] public bool Status { get; set; }
    [JsonProperty("token")] public string Token { get; set; } = string.Empty;
    [JsonProperty("image_profile_url")] public string ImageProfileUrl { get; set; } = string.Empty;
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deleted_at")] public DateTime? DeletedAt { get; set; }
}