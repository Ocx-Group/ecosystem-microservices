namespace Ecosystem.AccountService.Application.DTOs.Ticket;

public class TicketMessageDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string MessageContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsRead { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ImageProfileUrl { get; set; } = string.Empty;
}
