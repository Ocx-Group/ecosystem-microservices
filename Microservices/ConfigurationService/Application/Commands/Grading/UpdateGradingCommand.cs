using System.Text.Json.Serialization;
using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Grading;

public record UpdateGradingCommand : IRequest<GradingDto?>
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("name")] public string Name { get; init; } = null!;
    [JsonPropertyName("description")] public string? Description { get; init; }
    [JsonPropertyName("scope_level")] public int ScopeLevel { get; init; }
    [JsonPropertyName("is_infinity")] public bool IsInfinity { get; init; }
    [JsonPropertyName("personal_purchases")] public decimal PersonalPurchases { get; init; }
    [JsonPropertyName("personal_purchases_exact")] public bool PersonalPurchasesExact { get; init; }
    [JsonPropertyName("purchases_network")] public decimal PurchasesNetwork { get; init; }
    [JsonPropertyName("binary_volume")] public decimal BinaryVolume { get; init; }
    [JsonPropertyName("volume_points")] public int VolumePoints { get; init; }
    [JsonPropertyName("volume_points_network")] public int VolumePointsNetwork { get; init; }
    [JsonPropertyName("children_left_leg")] public int ChildrenLeftLeg { get; init; }
    [JsonPropertyName("children_right_leg")] public int ChildrenRightLeg { get; init; }
    [JsonPropertyName("front_by_matrix")] public int FrontByMatrix { get; init; }
    [JsonPropertyName("front_qualif_1")] public int FrontQualif1 { get; init; }
    [JsonPropertyName("front_score_1")] public int FrontScore1 { get; init; }
    [JsonPropertyName("front_qualif_2")] public int FrontQualif2 { get; init; }
    [JsonPropertyName("front_score_2")] public int FrontScore2 { get; init; }
    [JsonPropertyName("front_qualif_3")] public int FrontQualif3 { get; init; }
    [JsonPropertyName("front_score_3")] public int FrontScore3 { get; init; }
    [JsonPropertyName("exact_front_ratings")] public bool ExactFrontRatings { get; init; }
    [JsonPropertyName("leader_by_matrix")] public int LeaderByMatrix { get; init; }
    [JsonPropertyName("network_leaders")] public int? NetworkLeaders { get; init; }
    [JsonPropertyName("network_leaders_qualifier")] public int? NetworkLeadersQualifier { get; init; }
    [JsonPropertyName("products")] public int? Products { get; init; }
    [JsonPropertyName("affiliations")] public int? Affiliations { get; init; }
    [JsonPropertyName("have_both")] public bool HaveBoth { get; init; }
    [JsonPropertyName("activate_user_by")] public int ActivateUserBy { get; init; }
    [JsonPropertyName("active")] public int Active { get; init; }
    [JsonPropertyName("status")] public bool Status { get; init; }
    [JsonPropertyName("network_scope_level")] public int NetworkScopeLevel { get; init; }
    [JsonPropertyName("full_period")] public bool FullPeriod { get; init; }
}
