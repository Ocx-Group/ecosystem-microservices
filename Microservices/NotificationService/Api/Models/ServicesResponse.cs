using Newtonsoft.Json;

namespace Ecosystem.NotificationService.Api.Models;

public class ServicesResponse
{
    [JsonProperty("success")] public bool Success { get; set; }
    [JsonProperty("data")] public object? Data { get; set; }
    [JsonProperty("message")] public string Message { get; set; } = string.Empty;
    [JsonProperty("code")] public int Code { get; set; }
}
