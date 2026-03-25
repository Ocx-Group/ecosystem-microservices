namespace Ecosystem.AccountService.Domain.Models;

public partial class TicketMessage
{
    public long Id { get; set; }
    public long TicketId { get; set; }
    public int UserId { get; set; }
    public string MessageContent { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsRead { get; set; }
    public virtual Ticket Ticket { get; set; } = null!;
}
