
namespace Ecosystem.WalletService.Domain.Models;

public partial class WalletsPeriod
{
    public DateOnly Date { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long Id { get; set; }
}
