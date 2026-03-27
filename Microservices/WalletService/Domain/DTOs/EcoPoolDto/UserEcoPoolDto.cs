namespace Ecosystem.WalletService.Domain.DTOs.EcoPoolDto;

public class UserEcoPoolDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte AffiliateMode { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public int Father { get; set; }
    public int Sponsor { get; set; }
    public int BinarySponsor { get; set; }
    public byte BinaryMatrixSide { get; set; }
    public int ExternalGradingId { get; set; }
    public bool IsForcedComplete { get; set; }
    public bool IsBinaryEvaluated { get; set; }
    public int? ExternalProductId { get; set; }
}