using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class UserEcoPoolDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("user_name")] public string UserName { get; set; } = null!;
    [JsonProperty("level")] public int Level { get; set; }
    [JsonProperty("father")] public long Father { get; set; }
    [JsonProperty("side")] public int Side { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("family_tree")] public IEnumerable<UserEcoPoolDto> FamilyTree { get; set; } = [];
}
