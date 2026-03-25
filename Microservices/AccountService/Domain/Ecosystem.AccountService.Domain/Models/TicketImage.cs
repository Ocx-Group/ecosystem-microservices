namespace Ecosystem.AccountService.Domain.Models;

public partial class TicketImage
{
    public long Id { get; set; }
    public long TicketId { get; set; }
    public string ImagePath { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual Ticket Ticket { get; set; } = null!;
}
