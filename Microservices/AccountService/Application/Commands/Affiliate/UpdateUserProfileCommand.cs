using Ecosystem.AccountService.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateUserProfileCommand : IRequest<UsersAffiliatesDto?>
{
    public int Id { get; init; }

    [JsonProperty("identification")] public string? Identification { get; init; }

    [JsonProperty("binary_matrix_side")] public byte? BinaryMatrixSide { get; init; }

    [JsonProperty("address")] public string? Address { get; init; }

    [JsonProperty("phone")] public string? Phone { get; init; }

    [JsonProperty("zip_code")] public string? ZipCode { get; init; }

    [JsonProperty("country")] public int? Country { get; init; }

    [JsonProperty("birthday")] public DateTime? Birthday { get; init; }

    [JsonProperty("tax_id")] public string? TaxId { get; init; }

    [JsonProperty("legal_authorized_first")] public string? LegalAuthorizedFirst { get; init; }

    [JsonProperty("legal_authorized_second")] public string? LegalAuthorizedSecond { get; init; }

    [JsonProperty("beneficiary_name")] public string? BeneficiaryName { get; init; }

    [JsonProperty("beneficiary_email")] public string? BeneficiaryEmail { get; init; }

    [JsonProperty("beneficiary_phone")] public string? BeneficiaryPhone { get; init; }
}
