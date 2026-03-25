namespace Ecosystem.AccountService.Domain.Models;

public partial class AffiliatesAddressCoinpayRegister
{
    public long Id { get; set; }
    public decimal Monto { get; set; }
    public string Orden { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int TransactionId { get; set; }
    public int ExternalReferenceId { get; set; }
}
