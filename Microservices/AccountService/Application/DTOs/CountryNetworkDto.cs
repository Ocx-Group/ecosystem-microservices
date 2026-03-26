using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class CountryNetworkDto
{
    [JsonProperty("title")] public string Title { get; set; } = null!;
    [JsonProperty("value")] public int Value { get; set; }
    [JsonProperty("lat")] public double Lat { get; set; }
    [JsonProperty("lng")] public double Lng { get; set; }
}
