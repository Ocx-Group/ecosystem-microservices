namespace Ecosystem.WalletService.Domain.DTOs.BalanceInformationDto;

public class BalanceInformationDto
{
    public decimal? ReverseBalance { get; set; }

    public decimal? TotalAcquisitions { get; set; }

    public decimal? AvailableBalance { get; set; }

    public decimal? TotalCommissionsPaid { get; set; }
    public decimal? ServiceBalance { get; set; }
    public decimal BonusAmount { get; set; }
}