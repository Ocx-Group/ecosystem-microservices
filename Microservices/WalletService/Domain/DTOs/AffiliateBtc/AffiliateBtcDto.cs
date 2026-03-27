
using System.Text.Json.Serialization;

namespace Ecosystem.WalletService.Domain.DTOs.AffiliateBtc;

public class AffiliateBtcDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("affiliateId")] public int AffiliateId { get; set; }
    [JsonPropertyName("address")] public string Address { get; set; } = String.Empty;
    [JsonPropertyName("status")] public byte Status { get; set; }
    [JsonPropertyName("networkId")] public int NetworkId { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public DateTime? UpdatedAt { get; set; }
    [JsonPropertyName("deletedAt")] public DateTime? DeletedAt { get; set; }
}