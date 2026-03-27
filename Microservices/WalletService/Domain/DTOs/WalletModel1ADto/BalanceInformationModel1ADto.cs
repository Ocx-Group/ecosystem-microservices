namespace Ecosystem.WalletService.Domain.DTOs.WalletModel1ADto;

public class BalanceInformationModel1ADto
{
    public decimal? ReverseBalance { get; set; }

    public decimal? TotalAcquisitions { get; set; }

    public decimal? AvailableBalance { get; set; }

    public decimal? TotalCommissionsPaid { get; set; }
    public decimal? ServiceBalance { get; set; }
}