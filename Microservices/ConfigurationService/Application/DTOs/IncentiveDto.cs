using System.Text.Json.Serialization;

namespace Ecosystem.ConfigurationService.Application.DTOs;

public class IncentiveDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("scope_level")] public int ScopeLevel { get; set; }
    [JsonPropertyName("is_infinity")] public bool IsInfinity { get; set; }
    [JsonPropertyName("personal_purchases")] public decimal PersonalPurchases { get; set; }
    [JsonPropertyName("personal_purchases_exact")] public bool PersonalPurchasesExact { get; set; }
    [JsonPropertyName("purchases_network")] public decimal PurchasesNetwork { get; set; }
    [JsonPropertyName("binary_volume")] public decimal BinaryVolume { get; set; }
    [JsonPropertyName("volume_points")] public int VolumePoints { get; set; }
    [JsonPropertyName("volume_points_network")] public int VolumePointsNetwork { get; set; }
    [JsonPropertyName("children_left_leg")] public int ChildrenLeftLeg { get; set; }
    [JsonPropertyName("children_right_leg")] public int ChildrenRightLeg { get; set; }
    [JsonPropertyName("front_by_matrix")] public int FrontByMatrix { get; set; }
    [JsonPropertyName("front_qualif_1")] public int FrontQualif1 { get; set; }
    [JsonPropertyName("front_score_1")] public int FrontScore1 { get; set; }
    [JsonPropertyName("front_qualif_2")] public int FrontQualif2 { get; set; }
    [JsonPropertyName("front_score_2")] public int FrontScore2 { get; set; }
    [JsonPropertyName("front_qualif_3")] public int FrontQualif3 { get; set; }
    [JsonPropertyName("front_score_3")] public int FrontScore3 { get; set; }
    [JsonPropertyName("exact_front_ratings")] public bool ExactFrontRatings { get; set; }
    [JsonPropertyName("leader_by_matrix")] public int LeaderByMatrix { get; set; }
    [JsonPropertyName("network_leaders")] public int? NetworkLeaders { get; set; }
    [JsonPropertyName("network_leaders_qualifier")] public int? NetworkLeadersQualifier { get; set; }
    [JsonPropertyName("products")] public int? Products { get; set; }
    [JsonPropertyName("affiliations")] public int? Affiliations { get; set; }
    [JsonPropertyName("grading")] public int? Grading { get; set; }
    [JsonPropertyName("active")] public int Active { get; set; }
    [JsonPropertyName("status")] public bool Status { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonPropertyName("deleted_at")] public DateTime? DeletedAt { get; set; }
}
