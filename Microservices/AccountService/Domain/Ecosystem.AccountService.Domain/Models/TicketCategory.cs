namespace Ecosystem.AccountService.Domain.Models;

public partial class TicketCategory
{
    public long Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
