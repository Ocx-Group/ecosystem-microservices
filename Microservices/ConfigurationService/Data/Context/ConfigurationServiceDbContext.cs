using Ecosystem.ConfigurationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.ConfigurationService.Data.Context;

public partial class ConfigurationServiceDbContext : DbContext
{
    public ConfigurationServiceDbContext() { }

    public ConfigurationServiceDbContext(DbContextOptions<ConfigurationServiceDbContext> options)
        : base(options) { }

    public virtual DbSet<ApiClient> ApiClients { get; set; }
    public virtual DbSet<Bank> Banks { get; set; }
    public virtual DbSet<BankDetail> BankDetails { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Concepts> Concepts { get; set; }
    public virtual DbSet<ConceptConfigurations> ConceptConfigurations { get; set; }
    public virtual DbSet<Configurations> Configurations { get; set; }
    public virtual DbSet<Gradings> Gradings { get; set; }
    public virtual DbSet<Incentives> Incentives { get; set; }
    public virtual DbSet<PaidConcept> PaidConcepts { get; set; }
    public virtual DbSet<PaymentGroups> PaymentGroups { get; set; }
    public virtual DbSet<MatrixConfiguration> MatrixConfigurations { get; set; }
    public virtual DbSet<BrandConfiguration> BrandConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17090_pk__apiclien__3214ec073dd66e87");

            entity.ToTable("api_client", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('apiclient_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17104_pk__banks__3214ec0774204b7a");

            entity.ToTable("banks", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('banks_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BankName).HasColumnName("bank_name");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.KeyName).HasColumnName("key_name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17097_pk__bankdeta__3214ec07af03088a");

            entity.ToTable("bank_details", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('bankdetails_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Api).HasColumnName("api");
            entity.Property(e => e.BankId).HasColumnName("bank_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Bank).WithMany(p => p.BankDetails)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bankdetails_banks");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17111_pk__brand__3214ec07f9703688");

            entity.ToTable("brand", "configuration_service");

            entity.HasIndex(e => e.IsActive, "idx_17111_ix_brand_isactive").HasFilter("(is_active = true)");

            entity.HasIndex(e => e.Name, "idx_17111_ix_brand_name");

            entity.HasIndex(e => e.SecretKey, "idx_17111_uq_brand_secretkey").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('brand_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SecretKey).HasColumnName("secret_key");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Concepts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17128_pk__concepts__3214ec07928d73ac");

            entity.ToTable("concepts", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('concepts_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("active");
            entity.Property(e => e.BinaryGrades).HasColumnName("binary_grades");
            entity.Property(e => e.BinaryPercentage)
                .HasDefaultValueSql("0.00")
                .HasColumnName("binary_percentage");
            entity.Property(e => e.BinaryProductsGroup).HasColumnName("binary_products_group");
            entity.Property(e => e.BinaryTop)
                .HasDefaultValueSql("0.00")
                .HasColumnName("binary_top");
            entity.Property(e => e.BinaryTopInfinity)
                .HasDefaultValueSql("1")
                .HasColumnName("binary_top_infinity");
            entity.Property(e => e.BinaryType)
                .HasDefaultValueSql("1")
                .HasColumnName("binary_type");
            entity.Property(e => e.CalculateBy).HasColumnName("calculate_by");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateConcept).HasColumnName("date_concept");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Equalization).HasColumnName("equalization");
            entity.Property(e => e.IgnoreActivationOrder).HasColumnName("ignore_activation_order");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PayConcept).HasColumnName("pay_concept");
            entity.Property(e => e.PaymentGroupId).HasColumnName("payment_group_id");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.PaymentGroups).WithMany(p => p.Concepts)
                .HasForeignKey(d => d.PaymentGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_concepts_paymentgroups");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ConceptConfigurations>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17120_pk__conceptc__3214ec07f83e8c12");

            entity.ToTable("concept_configurations", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('conceptconfigurations_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.ConceptId).HasColumnName("concept_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Equalization).HasColumnName("equalization");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasOne(d => d.Concepts).WithMany(p => p.ConceptConfigurations)
                .HasForeignKey(d => d.ConceptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_concept_concept_configuration");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Configurations>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17144_pk__configur__3214ec07045d7c26");

            entity.ToTable("configurations", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('configurations_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DefaultValue).HasColumnName("default_value");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Gradings>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17151_pk__gradings__3214ec07da63777c");

            entity.ToTable("gradings", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('gradings_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ActivateUserBy)
                .HasDefaultValueSql("1")
                .HasColumnName("activate_user_by");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("1")
                .HasColumnName("active");
            entity.Property(e => e.Affiliations).HasColumnName("affiliations");
            entity.Property(e => e.BinaryVolume)
                .HasDefaultValueSql("0.00")
                .HasColumnName("binary_volume");
            entity.Property(e => e.ChildrenLeftLeg).HasColumnName("children_left_leg");
            entity.Property(e => e.ChildrenRightLeg).HasColumnName("children_right_leg");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExactFrontRatings).HasColumnName("exact_front_ratings");
            entity.Property(e => e.FrontByMatrix).HasColumnName("front_by_matrix");
            entity.Property(e => e.FrontQualif1).HasColumnName("front_qualif1");
            entity.Property(e => e.FrontScore1).HasColumnName("front_score1");
            entity.Property(e => e.FrontScore2).HasColumnName("front_score2");
            entity.Property(e => e.FrontScore3).HasColumnName("front_score3");
            entity.Property(e => e.Frontqualif2).HasColumnName("frontqualif2");
            entity.Property(e => e.Frontqualif3).HasColumnName("frontqualif3");
            entity.Property(e => e.FullPeriod).HasColumnName("full_period");
            entity.Property(e => e.HaveBoth).HasColumnName("have_both");
            entity.Property(e => e.IsInfinity).HasColumnName("is_infinity");
            entity.Property(e => e.LeaderByMatrix).HasColumnName("leader_by_matrix");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NetworkLeaders).HasColumnName("network_leaders");
            entity.Property(e => e.NetworkLeadersQualifier).HasColumnName("network_leaders_qualifier");
            entity.Property(e => e.NetworkScopeLevel).HasColumnName("network_scope_level");
            entity.Property(e => e.PersonalPurchases)
                .HasDefaultValueSql("0.00")
                .HasColumnName("personal_purchases");
            entity.Property(e => e.PersonalPurchasesExact).HasColumnName("personal_purchases_exact");
            entity.Property(e => e.Products).HasColumnName("products");
            entity.Property(e => e.PurchasesNetwork)
                .HasDefaultValueSql("0.00")
                .HasColumnName("purchases_network");
            entity.Property(e => e.ScopeLevel).HasColumnName("scope_level");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VolumePoints).HasColumnName("volume_points");
            entity.Property(e => e.VolumePointsNetwork).HasColumnName("volume_points_network");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Incentives>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17183_pk__incentiv__3214ec07093230c1");

            entity.ToTable("incentives", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('incentives_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("1")
                .HasColumnName("active");
            entity.Property(e => e.Affiliations).HasColumnName("affiliations");
            entity.Property(e => e.BinaryVolume)
                .HasDefaultValueSql("0.00")
                .HasColumnName("binary_volume");
            entity.Property(e => e.ChildrenLeftLeg).HasColumnName("children_left_leg");
            entity.Property(e => e.ChildrenRightLeg).HasColumnName("children_right_leg");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExactFrontRatings).HasColumnName("exact_front_ratings");
            entity.Property(e => e.FrontByMatrix).HasColumnName("front_by_matrix");
            entity.Property(e => e.FrontQuali2).HasColumnName("front_quali2");
            entity.Property(e => e.FrontQualif1).HasColumnName("front_qualif1");
            entity.Property(e => e.FrontQualif3).HasColumnName("front_qualif3");
            entity.Property(e => e.FrontScore1).HasColumnName("front_score1");
            entity.Property(e => e.FrontScore2).HasColumnName("front_score2");
            entity.Property(e => e.FrontScore3).HasColumnName("front_score3");
            entity.Property(e => e.Grading).HasColumnName("grading");
            entity.Property(e => e.IsInfinity).HasColumnName("is_infinity");
            entity.Property(e => e.LeaderByMatrix).HasColumnName("leader_by_matrix");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NetworkLeaders).HasColumnName("network_leaders");
            entity.Property(e => e.NetworkLeadersQualifier).HasColumnName("network_leaders_qualifier");
            entity.Property(e => e.PersonalPurchases)
                .HasDefaultValueSql("0.00")
                .HasColumnName("personal_purchases");
            entity.Property(e => e.PersonalPurchasesExact).HasColumnName("personal_purchases_exact");
            entity.Property(e => e.Products).HasColumnName("products");
            entity.Property(e => e.PurchasesNetwork)
                .HasDefaultValueSql("0.00")
                .HasColumnName("purchases_network");
            entity.Property(e => e.ScopeLevel).HasColumnName("scope_level");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VolumePoints).HasColumnName("volume_points");
            entity.Property(e => e.VolumePointsNetwork).HasColumnName("volume_points_network");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<PaidConcept>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17211_pk__paidconc__3214ec073602c932");

            entity.ToTable("paid_concepts", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('paidconcepts_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ConceptId).HasColumnName("concept_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasOne(d => d.Concepts).WithMany(p => p.PaidConcepts)
                .HasForeignKey(d => d.ConceptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_paidconcepts_concepts");
        });

        modelBuilder.Entity<PaymentGroups>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17218_pk__paymentg__3214ec07f79b91ee");

            entity.ToTable("payment_groups", "configuration_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('paymentgroups_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<MatrixConfiguration>(entity =>
        {
            entity.HasKey(e => e.MatrixType).HasName("matrix_config_pkey");
            entity.ToTable("matrix_config", "configuration_service");

            entity.Property(e => e.MatrixType).HasColumnName("matrix_type");
            entity.Property(e => e.Threshold).HasColumnName("threshold");
            entity.Property(e => e.FeeAmount).HasColumnName("fee_amount");
            entity.Property(e => e.MinWithdraw).HasColumnName("min_withdraw");
            entity.Property(e => e.MaxWithdraw).HasColumnName("max_withdraw");
            entity.Property(e => e.ChildCount).HasColumnName("child_count");
            entity.Property(e => e.RangeMin).HasColumnName("range_min");
            entity.Property(e => e.RangeMax).HasColumnName("range_max");
            entity.Property(e => e.Levels).HasColumnName("levels");
            entity.Property(e => e.MatrixName).HasMaxLength(50).HasColumnName("matrix_name");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<BrandConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("brand_configuration_pkey");

            entity.ToTable("brand_configuration", "configuration_service");

            entity.HasIndex(e => e.BrandId, "idx_brand_configuration_brand_id").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('brand_configuration_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.AdminUserName).HasColumnName("admin_user_name");

            // Email / notifications
            entity.Property(e => e.SenderName).HasColumnName("sender_name");
            entity.Property(e => e.SenderEmail).HasColumnName("sender_email");
            entity.Property(e => e.EmailTemplateFolder).HasColumnName("email_template_folder");

            // Frontend
            entity.Property(e => e.ClientUrl).HasColumnName("client_url");

            // Commission
            entity.Property(e => e.CommissionEnabled).HasColumnName("commission_enabled");
            entity.Property(e => e.CommissionLevelsJson)
                .HasColumnType("jsonb")
                .HasColumnName("commission_levels");
            entity.Property(e => e.BonusPercentage)
                .HasDefaultValueSql("0")
                .HasColumnName("bonus_percentage");

            // PDF / Invoice
            entity.Property(e => e.PdfTemplateName).HasColumnName("pdf_template_name");
            entity.Property(e => e.CompanyName).HasColumnName("company_name");
            entity.Property(e => e.CompanyIdentifier).HasColumnName("company_identifier");
            entity.Property(e => e.SupportEmail).HasColumnName("support_email");
            entity.Property(e => e.SupportPhone).HasColumnName("support_phone");
            entity.Property(e => e.DocumentType).HasColumnName("document_type");
            entity.Property(e => e.LogoUrl).HasColumnName("logo_url");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.BackgroundColor).HasColumnName("background_color");

            // Affiliate tree
            entity.Property(e => e.DefaultFatherAffiliateId).HasColumnName("default_father_affiliate_id");
            entity.Property(e => e.ActivateOnRegistration)
                .HasDefaultValueSql("true")
                .HasColumnName("activate_on_registration");

            // Payment groups
            entity.Property(e => e.DefaultPaymentGroupId).HasColumnName("default_payment_group_id");
            entity.Property(e => e.TradingAcademyPaymentGroupId).HasColumnName("trading_academy_payment_group_id");

            // Withdrawal rules
            entity.Property(e => e.WithdrawalValidationType)
                .HasDefaultValue("None")
                .HasColumnName("withdrawal_validation_type");
            entity.Property(e => e.WithdrawalTimeZone).HasColumnName("withdrawal_time_zone");
            entity.Property(e => e.WithdrawalStartHour).HasColumnName("withdrawal_start_hour");
            entity.Property(e => e.WithdrawalEndHour).HasColumnName("withdrawal_end_hour");
            entity.Property(e => e.WithdrawalCapNoDirects).HasColumnName("withdrawal_cap_no_directs");
            entity.Property(e => e.Requires10PercentPurchaseRule).HasColumnName("requires_10_percent_purchase_rule");
            entity.Property(e => e.PoolValidationRequired).HasColumnName("pool_validation_required");

            // Crypto / ConPayment
            entity.Property(e => e.ConPaymentEnabled).HasColumnName("con_payment_enabled");
            entity.Property(e => e.ConPaymentAddress).HasColumnName("con_payment_address");
            entity.Property(e => e.BlockchainNetworkId).HasColumnName("blockchain_network_id");

            // Status & audit
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasOne(d => d.Brand).WithMany()
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_brand_configuration_brand");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
