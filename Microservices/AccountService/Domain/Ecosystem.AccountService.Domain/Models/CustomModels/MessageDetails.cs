using System.ComponentModel.DataAnnotations;

namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class MessageDetails
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    [MaxLength(255)]
    public string MessageContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsRead { get; set; }
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? ImageProfileUrl { get; set; } = string.Empty;
}
