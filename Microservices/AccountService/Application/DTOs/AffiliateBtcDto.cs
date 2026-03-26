namespace Ecosystem.AccountService.Application.DTOs;

public class AffiliateBtcDto
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public string Address { get; set; } = string.Empty;
    public short Status { get; set; }
    public long? NetworkId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
