using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class NetworkDetailsDto
{
    [JsonProperty("model123")] public StatisticsModel12356Dto Model123 { get; set; } = new();
    [JsonProperty("model4")] public StatisticsModel4Dto Model4 { get; set; } = new();
    [JsonProperty("model5")] public StatisticsModel12356Dto Model5 { get; set; } = new();
    [JsonProperty("model6")] public StatisticsModel12356Dto Model6 { get; set; } = new();
}

public class StatisticsModel12356Dto
{
    [JsonProperty("direct_affiliates")] public int DirectAffiliates { get; set; }
    [JsonProperty("indirect_affiliates")] public int IndirectAffiliates { get; set; }
}

public class StatisticsModel4Dto
{
    [JsonProperty("left_count")] public int LeftCount { get; set; }
    [JsonProperty("right_count")] public int RightCount { get; set; }
}
