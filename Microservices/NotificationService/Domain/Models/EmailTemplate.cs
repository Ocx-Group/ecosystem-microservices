using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecosystem.NotificationService.Domain.Models;

public class EmailTemplate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string TemplateKey { get; set; } = null!;

    public long BrandId { get; set; }

    public string Subject { get; set; } = null!;

    public string HtmlBody { get; set; } = null!;

    public List<string> Placeholders { get; set; } = [];

    public bool IsActive { get; set; } = true;

    public int Version { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
