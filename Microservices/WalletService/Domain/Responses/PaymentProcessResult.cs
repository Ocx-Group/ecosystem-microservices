using System.ComponentModel.DataAnnotations.Schema;
namespace Ecosystem.WalletService.Domain.Responses;

public class PaymentProcessResult
{
    [Column("success")]
    public bool Success { get; set; }

    [Column("processed_count")]
    public int ProcessedCount { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }
}