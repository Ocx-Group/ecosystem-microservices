namespace Ecosystem.NotificationService.Data.Settings;

public class MongoDbSettings
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}
