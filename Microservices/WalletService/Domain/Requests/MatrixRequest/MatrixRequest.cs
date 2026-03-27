namespace Ecosystem.WalletService.Domain.Requests.MatrixRequest;

public class MatrixRequest
{
    public int UserId { get; set; }
    public int MatrixType { get; set; }
    public int? RecipientId { get; set; }
    public int? Cycle { get; set; }
}