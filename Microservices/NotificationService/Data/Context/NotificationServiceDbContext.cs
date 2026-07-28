using Ecosystem.NotificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ecosystem.NotificationService.Data.Context;

public class NotificationServiceDbContext : DbContext
{
    private const string SchemaName          = "notification_service";
    private const string ColCreatedAt        = "created_at";
    private const string ColUpdatedAt        = "updated_at";
    private const string ColDeletedAt        = "deleted_at";
    private const string ColBrandId          = "brand_id";
    private const string ColIsActive         = "is_active";
    private const string SqlCurrentTimestamp = "CURRENT_TIMESTAMP";

    public NotificationServiceDbContext() { }

    public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options)
        : base(options) { }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<ApiClient> ApiClients { get; set; }

    private static readonly ValueConverter<DateTime, DateTime> UtcDateTimeConverter = new(
        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    private static readonly ValueConverter<DateTime?, DateTime?> NullableUtcDateTimeConverter = new(
        v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)) : v,
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt).HasConversion(UtcDateTimeConverter);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt).HasConversion(NullableUtcDateTimeConverter);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("brands", SchemaName);
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SecretKey).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasColumnName("name");
            entity.Property(e => e.SecretKey).IsRequired().HasColumnName("secret_key");
            entity.Property(e => e.IsActive).HasDefaultValueSql("true").HasColumnName(ColIsActive);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt).HasConversion(UtcDateTimeConverter);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt).HasConversion(NullableUtcDateTimeConverter);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt).HasConversion(NullableUtcDateTimeConverter);
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.ToTable("api_clients", SchemaName);
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasColumnName("name");
            entity.Property(e => e.Token).IsRequired().HasColumnName("token");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt).HasConversion(UtcDateTimeConverter);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt).HasConversion(NullableUtcDateTimeConverter);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt).HasConversion(NullableUtcDateTimeConverter);
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
    }
}
