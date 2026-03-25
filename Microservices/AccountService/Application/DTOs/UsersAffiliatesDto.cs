using Ecosystem.AccountService.Domain.Constants;
using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.DTOs;

public class UsersAffiliatesDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("user_name")] public string UserName { get; set; } = string.Empty;
    [JsonProperty("email")] public string Email { get; set; } = string.Empty;
    [JsonProperty("is_affiliate")] public sbyte IsAffiliate { get; set; } = 1;
    [JsonProperty("identification_type")] public byte IdentificationType { get; set; }
    [JsonProperty("affiliate_mode")] public byte AffiliateMode { get; set; }
    [JsonProperty("identification")] public string Identification { get; set; } = string.Empty;
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("last_name")] public string? LastName { get; set; }
    [JsonProperty("secret_question")] public string? SecretQuestion { get; set; }
    [JsonProperty("secret_answer")] public string? SecretAnswer { get; set; }
    [JsonProperty("address")] public string? Address { get; set; }
    [JsonProperty("phone")] public string? Phone { get; set; }
    [JsonProperty("zip_code")] public string? ZipCode { get; set; }
    [JsonProperty("country")] public int Country { get; set; }
    [JsonProperty("state_place")] public string? StatePlace { get; set; }
    [JsonProperty("city")] public string? City { get; set; }
    [JsonProperty("birthday")] public DateTime? Birthday { get; set; }
    [JsonProperty("tax_id")] public string? TaxId { get; set; }
    [JsonProperty("beneficiary_name")] public string? BeneficiaryName { get; set; }
    [JsonProperty("legal_authorized_first")] public string? LegalAuthorizedFirst { get; set; }
    [JsonProperty("legal_authorized_second")] public string? LegalAuthorizedSecond { get; set; }
    [JsonProperty("status")] public byte Status { get; set; }
    [JsonProperty("affiliate_type")] public string? AffiliateType { get; set; }
    [JsonProperty("activation_date")] public DateTime? ActivationDate { get; set; }
    [JsonProperty("email_verification")] public bool EmailVerification { get; set; }
    [JsonProperty("father")] public int Father { get; set; }
    [JsonProperty("sponsor")] public int Sponsor { get; set; }
    [JsonProperty("binary_sponsor")] public int BinarySponsor { get; set; }
    [JsonProperty("binary_matrix_side")] public byte BinaryMatrixSide { get; set; }
    [JsonProperty("side")] public byte Side { get; set; }
    [JsonProperty("authorization_date")] public DateTime? AuthorizationDate { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("image_profile_url")] public string? ImageProfileUrl { get; set; }
    [JsonProperty("father_user")] public UsersAffiliatesDto? FatherUser { get; set; }
    [JsonProperty("father_user_level")] public UsersAffiliatesDto? FatherUserUniLevel { get; set; }
    [JsonProperty("verification_code")] public string? VerificationCode { get; set; }
    [JsonProperty("image_id_path")] public string? ImageIdPath { get; set; }
    [JsonProperty("card_id_message")] public string? CardIdMessage { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("deleted_at")] public DateTime? DeletedAt { get; set; }
    [JsonProperty("card_id_authorization")] public bool CardIdAuthorization { get; set; }
    [JsonProperty("external_grading_id")] public int? ExternalGradingId { get; set; }
    [JsonProperty("external_grading_before_id")] public int? ExternalGradingIdBefore { get; set; }
    [JsonProperty("is_forced_complete")] public bool IsForcedComplete { get; set; }
    [JsonProperty("is_binary_evaluated")] public bool IsBinaryEvaluated { get; set; }
    [JsonProperty("external_product_id")] public int? ExternalProductId { get; set; }
    [JsonProperty("google_auth_code")] public string? GoogleAuthCode { get; set; }
    [JsonProperty("is_google_auth")] public bool? IsGoogleAuth { get; set; }
    [JsonProperty("attempts")] public bool? Attempts { get; set; }
    [JsonProperty("use_pin")] public bool UsePin { get; set; }
    [JsonProperty("security_pin")] public string? SecurityPin { get; set; }
    [JsonProperty("status_activation")] public string? StatusActivation { get; set; }
    [JsonProperty("message_alert")] public byte MessageAlert { get; set; }
    [JsonProperty("active_cap")] public byte ActiveCap { get; set; }
    [JsonProperty("country_navigation")] public CountryDto CountryNavigation { get; set; } = null!;
    [JsonProperty("termsConditions")] public bool TermsConditions { get; set; }

    [JsonProperty("binary_side_name")]
    public string BinarySideName
        => BinaryMatrixSide switch
        {
            1 => AccountServiceConstants.BinarySideArray[1],
            2 => AccountServiceConstants.BinarySideArray[2],
            _ => AccountServiceConstants.BinarySideArray[0]
        };

    public ICollection<UsersAffiliatesDto> ChildrenBinarySponsor { get; set; } = [];
    public ICollection<UsersAffiliatesDto> ChildrenUniLevelSponsor { get; set; } = [];

    [JsonProperty("beneficiary_email")] public string? BeneficiaryEmail { get; set; }
    [JsonProperty("beneficiary_phone")] public string? BeneficiaryPhone { get; set; }
}