using System;
using System.Collections.Generic;

namespace Ecosystem.WalletService.Domain.Models;

public partial class Bonuse
{
    public long BonusId { get; set; }

    public int AffiliateId { get; set; }

    public decimal CurrentAmount { get; set; }

    public short Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<BonusTransactionHistory> BonusTransactionHistories { get; } = new List<BonusTransactionHistory>();
}
