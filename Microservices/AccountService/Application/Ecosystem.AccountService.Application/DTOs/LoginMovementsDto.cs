using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class LoginMovementsDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("affiliateId")] public int AffiliateId { get; set; }
    [JsonProperty("browserInfo")] public string? BrowserInfo { get; set; }
    [JsonProperty("operatingSystem")] public string? OperatingSystem { get; set; }
    [JsonProperty("ipAddress")] public string IpAddress { get; set; } = string.Empty;
    [JsonProperty("status")] public bool Status { get; set; }
    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deletedAt")] public DateTime? DeletedAt { get; set; }
}