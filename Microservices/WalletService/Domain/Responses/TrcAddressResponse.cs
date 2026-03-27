namespace Ecosystem.WalletService.Domain.Responses;

public class TrcAddressResponse
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public string Address { get; set; } = null!;
    public bool Status { get; set; }
}