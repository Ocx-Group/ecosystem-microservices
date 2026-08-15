using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record CreateAffiliateCommand : IRequest<ServicesResponse>
{
    [JsonProperty("user_name")] public string? UserName { get; init; }

    [JsonProperty("name")] public string? Name { get; init; }

    [JsonProperty("password")] public string? Password { get; init; }

    [JsonProperty("last_name")] public string? LastName { get; init; }

    [JsonProperty("email")] public string? Email { get; init; }

    [JsonProperty("country")] public int Country { get; init; }

    [JsonProperty("affiliate_type")] public string? AffiliateType { get; init; }

    [JsonProperty("father")] public int Father { get; init; }

    [JsonProperty("sponsor")] public int? Sponsor { get; init; }

    [JsonProperty("binary_sponsor")] public int? BinarySponsor { get; init; }

    [JsonProperty("phone")] public string? Phone { get; init; }

    [JsonProperty("state_place")] public string? StatePlace { get; init; }

    [JsonProperty("city")] public string? City { get; init; }

    [JsonProperty("binary_matrix_side")] public byte? BinaryMatrixSide { get; init; }

    [JsonProperty("status")] public byte? Status { get; init; }

    [JsonProperty("termsConditions")] public bool TermsConditions { get; init; }
}
