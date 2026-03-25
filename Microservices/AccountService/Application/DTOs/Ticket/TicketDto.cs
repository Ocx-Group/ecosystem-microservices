namespace Ecosystem.AccountService.Application.DTOs.Ticket;

public class TicketDto
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public int? CategoryId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public bool Status { get; set; }
    public bool IsRead { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public virtual ICollection<TicketImagesDto> TicketImages { get; set; } = new List<TicketImagesDto>();
    public virtual ICollection<TicketMessageDto> Messages { get; set; } = new List<TicketMessageDto>();
}
