using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.AccountService.Domain.Models;

public class Ticket
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public long? CategoryId { get; set; }
    public string Subject { get; set; } = null!;
    public bool? Status { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Description { get; set; } = null!;
    public long BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;
    public virtual TicketCategory? Category { get; set; }

    [Column("ticket_images")]
    public virtual ICollection<TicketImage> TicketImages { get; set; } = new List<TicketImage>();
    public virtual ICollection<TicketMessage> TicketMessages { get; set; } = new List<TicketMessage>();
}
