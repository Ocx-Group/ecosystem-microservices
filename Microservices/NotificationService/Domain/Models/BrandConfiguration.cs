using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecosystem.NotificationService.Domain.Models;

public class BrandConfiguration
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public long BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string SenderName { get; set; } = null!;

    public string SenderEmail { get; set; } = null!;

    public string? SupportEmail { get; set; }

    public string? ClientUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
