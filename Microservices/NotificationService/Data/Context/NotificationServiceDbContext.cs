using Ecosystem.NotificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.NotificationService.Data.Context;

public class NotificationServiceDbContext : DbContext
{
    private const string SchemaName          = "notification_service";
    private const string ColCreatedAt        = "created_at";
    private const string ColUpdatedAt        = "updated_at";
    private const string ColBrandId          = "brand_id";
    private const string ColIsActive         = "is_active";
    private const string SqlCurrentTimestamp = "CURRENT_TIMESTAMP";

    public NotificationServiceDbContext() { }

    public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options)
        : base(options) { }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
    public virtual DbSet<BrandConfiguration> BrandConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("email_templates_pkey");
            entity.ToTable("email_templates", SchemaName);
            entity.HasIndex(e => new { e.TemplateKey, e.BrandId }, "uq_email_templates_key_brand").IsUnique();
            entity.HasIndex(e => e.BrandId, "ix_email_templates_brand_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('notification_service.email_templates_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.TemplateKey).IsRequired().HasMaxLength(100).HasColumnName("template_key");
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);
            entity.Property(e => e.Subject).IsRequired().HasMaxLength(255).HasColumnName("subject");
            entity.Property(e => e.HtmlBody).IsRequired().HasColumnName("html_body");
            entity.Property(e => e.Placeholders)
                .HasColumnName("placeholders")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (a, b) => (a ?? new()).SequenceEqual(b ?? new()),
                    v => v.Aggregate(0, (acc, s) => HashCode.Combine(acc, s.GetHashCode())),
                    v => v.ToList()));
            entity.Property(e => e.IsActive).HasDefaultValueSql("true").HasColumnName(ColIsActive);
            entity.Property(e => e.Version).HasDefaultValueSql("1").HasColumnName("version");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
        });

        modelBuilder.Entity<BrandConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("brand_configurations_pkey");
            entity.ToTable("brand_configurations", SchemaName);
            entity.HasIndex(e => e.BrandId, "uq_brand_configurations_brand_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('notification_service.brand_configurations_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.SenderName).IsRequired().HasMaxLength(100).HasColumnName("sender_name");
            entity.Property(e => e.SenderEmail).IsRequired().HasMaxLength(255).HasColumnName("sender_email");
            entity.Property(e => e.SupportEmail).HasMaxLength(255).HasColumnName("support_email");
            entity.Property(e => e.ClientUrl).HasMaxLength(500).HasColumnName("client_url");
            entity.Property(e => e.IsActive).HasDefaultValueSql("true").HasColumnName(ColIsActive);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
        });
    }
}
