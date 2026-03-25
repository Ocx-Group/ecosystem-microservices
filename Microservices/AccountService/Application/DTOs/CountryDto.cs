using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class CountryDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = null!;
    [JsonProperty("iso3")] public string? Iso3 { get; set; }
    [JsonProperty("numericCode")] public string? NumericCode { get; set; }
    [JsonProperty("iso2")] public string? Iso2 { get; set; }
    [JsonProperty("phoneCode")] public string? PhoneCode { get; set; }
    [JsonProperty("capital")] public string? Capital { get; set; }
    [JsonProperty("currency")] public string? Currency { get; set; }
    [JsonProperty("currencyName")] public string? CurrencyName { get; set; }
    [JsonProperty("currencySymbol")] public string? CurrencySymbol { get; set; }
    [JsonProperty("tld")] public string? Tld { get; set; }
    [JsonProperty("native")] public string? Native { get; set; }
    [JsonProperty("region")] public string? Region { get; set; }
    [JsonProperty("subRegion")] public string? SubRegion { get; set; }
    [JsonProperty("latitude")] public decimal? Latitude { get; set; }
    [JsonProperty("longitude")] public decimal? Longitude { get; set; }
    [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deletedAt")] public DateTime? DeletedAt { get; set; }
}