namespace Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;

public class WalletPeriodDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

}