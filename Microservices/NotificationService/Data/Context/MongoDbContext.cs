using Ecosystem.NotificationService.Data.Settings;
using Ecosystem.NotificationService.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecosystem.NotificationService.Data.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<EmailTemplate> EmailTemplates
        => _database.GetCollection<EmailTemplate>("email_templates");

    public IMongoCollection<BrandConfiguration> BrandConfigurations
        => _database.GetCollection<BrandConfiguration>("brand_configurations");
}
