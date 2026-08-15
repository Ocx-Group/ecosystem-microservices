using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonProperty("identification")] public string? Identification { get; init; }

    [JsonProperty("name")] public string? Name { get; init; }

    [JsonProperty("user_name")] public string? UserName { get; init; }

    [JsonProperty("last_name")] public string? LastName { get; init; }

    [JsonProperty("address")] public string? Address { get; init; }

    [JsonProperty("legal_authorized_first")] public string? LegalAuthorizedFirst { get; init; }

    [JsonProperty("legal_authorized_second")] public string? LegalAuthorizedSecond { get; init; }

    [JsonProperty("phone")] public string? Phone { get; init; }

    [JsonProperty("email")] public string? Email { get; init; }

    [JsonProperty("zip_code")] public string? ZipCode { get; init; }

    [JsonProperty("country")] public int? Country { get; init; }

    [JsonProperty("state_place")] public string? StatePlace { get; init; }

    [JsonProperty("city")] public string? City { get; init; }

    [JsonProperty("birthday")] public DateTime? Birthday { get; init; }

    [JsonProperty("tax_id")] public string? TaxId { get; init; }

    [JsonProperty("beneficiary_name")] public string? BeneficiaryName { get; init; }

    [JsonProperty("status")] public byte? Status { get; init; }

    [JsonProperty("affiliate_type")] public string? AffiliateType { get; init; }

    [JsonProperty("father")] public int? Father { get; init; }

    [JsonProperty("sponsor")] public int? Sponsor { get; init; }

    [JsonProperty("termsConditions")] public bool? TermsConditions { get; init; }

    [JsonProperty("beneficiary_email")] public string? BeneficiaryEmail { get; init; }

    [JsonProperty("beneficiary_phone")] public string? BeneficiaryPhone { get; init; }
}
