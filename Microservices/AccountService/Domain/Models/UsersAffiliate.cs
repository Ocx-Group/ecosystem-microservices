namespace Ecosystem.AccountService.Domain.Models;

public partial class UsersAffiliate
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public short IdentificationType { get; set; }
    public short AffiliateMode { get; set; }
    public string Identification { get; set; } = null!;
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Zipcode { get; set; }
    public int Country { get; set; }
    public string? StatePlace { get; set; }
    public string? City { get; set; }
    public short Status { get; set; }
    public DateTime? ActivationDate { get; set; }
    public bool EmailVerification { get; set; }
    public int Father { get; set; }
    public int Sponsor { get; set; }
    public int BinarySponsor { get; set; }
    public short BinaryMatrixSide { get; set; }
    public short Side { get; set; }
    public string? ImagePathId { get; set; }
    public DateTime? AuthorizationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? ImageProfileUrl { get; set; }
    public string? VerificationCode { get; set; }
    public bool CardIdAuthorization { get; set; }
    public string? CardIdMessage { get; set; }
    public int? ExternalGradingId { get; set; }
    public bool IsForcedComplete { get; set; }
    public bool IsBinaryEvaluated { get; set; }
    public int? ExternalProductId { get; set; }
    public string? GoogleAuthCode { get; set; }
    public bool? IsGoogleAuth { get; set; }
    public int? Attempts { get; set; }
    public bool Usepin { get; set; }
    public string? SecurityPin { get; set; }
    public string PrivateKey { get; set; } = null!;
    public short MessageAlert { get; set; }
    public decimal? CoinpaymentsCap { get; set; }
    public string? PasswordTemp { get; set; }
    public short ActiveCap { get; set; }
    public int? ExternalGradingIdBefore { get; set; }
    public DateTime? Birthday { get; set; }
    public string? TaxId { get; set; }
    public string? BeneficiaryName { get; set; }
    public string? LegalAuthorizedFirst { get; set; }
    public string? LegalAuthorizedSecond { get; set; }
    public string? StatusActivation { get; set; }
    public string? AffiliateType { get; set; }
    public bool TermsConditions { get; set; }
    public long BrandId { get; set; }
    public string? BeneficiaryEmail { get; set; }
    public string? BeneficiaryPhone { get; set; }

    public virtual ICollection<AffiliatesAddress> AffiliatesAddresses { get; } = new List<AffiliatesAddress>();
    public virtual ICollection<AffiliatesBtc> AffiliatesBtcs { get; } = new List<AffiliatesBtc>();
    public virtual ICollection<AffiliatesCountRegister> AffiliatesCountRegisters { get; } = new List<AffiliatesCountRegister>();
    public virtual Brand Brand { get; set; } = null!;
    public virtual Country CountryNavigation { get; set; } = null!;
    public virtual ICollection<MatrixPosition> MatrixPositions { get; } = new List<MatrixPosition>();
}
