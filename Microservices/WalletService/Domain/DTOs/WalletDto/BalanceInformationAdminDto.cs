namespace Ecosystem.WalletService.Domain.DTOs.WalletDto;

public class BalanceInformationAdminDto
{
    public decimal WalletProfit { get; set; }
    public decimal CommissionsPaid { get; set; }
    public decimal CalculatedCommissions { get; set; }
    public int EnabledAffiliates { get; set; }
    public decimal TotalReverseBalance { get; set; }
    
    public decimal TotalCommissionsEarned { get; set; }
}