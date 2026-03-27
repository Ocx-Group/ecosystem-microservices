using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;
using TransactionType = Ecosystem.WalletService.Domain.Models.TransactionType;

namespace Ecosystem.WalletService.Data.Context;

public partial class WalletServiceDbContext : DbContext
{
    public WalletServiceDbContext()
    {
    }

    public WalletServiceDbContext(DbContextOptions<WalletServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiClient> ApiClient { get; set; }

    public virtual DbSet<BonusTransactionHistory> BonusTransactionHistories { get; set; }

    public virtual DbSet<Bonuse> Bonuses { get; set; }

    public virtual DbSet<Brand> Brand { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoicePack> InvoicePacks { get; set; }

    public virtual DbSet<InvoicePacksDetail> InvoicePacksDetails { get; set; }

    public virtual DbSet<InvoicesDetail> InvoicesDetails { get; set; }

    public virtual DbSet<LeaderBoardModel5> LeaderBoardModel5 { get; set; }

    public virtual DbSet<LeaderBoardModel6> LeaderBoardModel6 { get; set; }

    public virtual DbSet<ModelConfiguration> ModelConfiguration { get; set; }

    public virtual DbSet<ModelConfigurationLevel> ModelConfigurationLevels { get; set; }

    public virtual DbSet<ModelFourStatistic> ModelFourStatistics { get; set; }

    public virtual DbSet<NetworkPurchase> NetworkPurchases { get; set; }

    public virtual DbSet<ResultsModel1a> ResultsModel1A { get; set; }

    public virtual DbSet<ResultsModel1aLevel> ResultsModel1ALevels { get; set; }

    public virtual DbSet<ResultsModel1b> ResultsModel1B { get; set; }

    public virtual DbSet<ResultsModel1bLevel> ResultsModel1BLevels { get; set; }

    public virtual DbSet<ResultsModel2> ResultsModel2 { get; set; }

    public virtual DbSet<ResultsModel2Level> ResultsModel2Levels { get; set; }

    public virtual DbSet<ResultsModel3> ResultsModel3 { get; set; }

    public virtual DbSet<ResultsModel3Level> ResultsModel3Levels { get; set; }

    public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<VolumePurchase> VolumePurchases { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletsHistory> WalletsHistories { get; set; }

    public virtual DbSet<WalletsModel1a> WalletsModel1A { get; set; }

    public virtual DbSet<WalletsModel1b> WalletsModel1B { get; set; }

    public virtual DbSet<WalletsPeriod> WalletsPeriods { get; set; }

    public virtual DbSet<WalletsRequest> WalletsRequests { get; set; }

    public virtual DbSet<WalletsRetentionsConfig> WalletsRetentionsConfigs { get; set; }

    public virtual DbSet<WalletsServiceModel1a> WalletsServiceModel1A { get; set; }

    public virtual DbSet<WalletsServiceModel1b> WalletsServiceModel1B { get; set; }

    public virtual DbSet<WalletsServiceModel2> WalletsServiceModel2 { get; set; }

    public virtual DbSet<WalletsWait> WalletsWaits { get; set; }
    public virtual DbSet<WalletsWithdrawal> WalletsWithdrawals { get; set; }

    public DbSet<InvoicesSpResponse> InvoicesSpResponses { get; set; }
    public DbSet<PurchasesPerMonth> PurchasesPerMonth { get; set; }
    public DbSet<MatrixQualification> MatrixQualifications { get; set; }
    public DbSet<MatrixEarning> MatrixEarnings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletServiceDbContext).Assembly);
        modelBuilder.Entity<PaymentProcessResult>().HasNoKey();
        
        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17275_pk__apiclien__3214ec077cd5fa1f");

            entity.ToTable("api_client", "wallet_service");

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

        modelBuilder.Entity<BonusTransactionHistory>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("idx_17289_pk__bonustra__55433a6be74d1473");

            entity.ToTable("bonus_transaction_history", "wallet_service");

            entity.Property(e => e.TransactionId)
                .HasDefaultValueSql("nextval('bonustransactionhistory_transactionid_seq'::regclass)")
                .HasColumnName("transaction_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BonusId).HasColumnName("bonus_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Bonus).WithMany(p => p.BonusTransactionHistories)
                .HasForeignKey(d => d.BonusId)
                .HasConstraintName("fk__bonustran__bonus__038683f8");

            entity.HasOne(d => d.Invoice).WithMany(p => p.BonusTransactionHistories)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("fk__bonustran__invoi__047aa831");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.BonusTransactionHistories)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("fk__bonustran__trans__056ecc6a");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Bonuse>(entity =>
        {
            entity.HasKey(e => e.BonusId).HasName("idx_17282_pk__bonuses__8e554768a9aa89ac");

            entity.ToTable("bonuses", "wallet_service");

            entity.Property(e => e.BonusId)
                .HasDefaultValueSql("nextval('bonuses_bonusid_seq'::regclass)")
                .HasColumnName("bonus_id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CurrentAmount).HasColumnName("current_amount");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17296_pk__brand__3214ec07006c5c8a");

            entity.ToTable("brand", "wallet_service");

            entity.HasIndex(e => e.IsActive, "idx_17296_ix_brand_isactive").HasFilter("(is_active = true)");

            entity.HasIndex(e => e.Name, "idx_17296_ix_brand_name");

            entity.HasIndex(e => e.SecretKey, "idx_17296_uq_brand_secretkey").IsUnique();

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

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17305_pk__coinpaym__3214ec07bb110a2b");

            entity.ToTable("transactions", "wallet_service");

            entity.HasIndex(e => e.IdTransaction, "idx_17305_uq__coinpaym__45542f44f9ec0298").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('coinpaymenttransactions_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Acredited).HasColumnName("acredited");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.AmountReceived)
                .HasDefaultValueSql("0.00")
                .HasColumnName("amount_received");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.IdTransaction).HasColumnName("id_transaction");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.Products).HasColumnName("products");
            entity.Property(e => e.Reference).HasColumnName("reference");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Address).HasColumnName("address");
            
            entity.HasOne(d => d.Brand).WithMany(p => p.CoinpaymentTransactions)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_coinpaymenttransactions_brand");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Commission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17314_pk__commissi__3214ec071e266627");

            entity.ToTable("commissions", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('commissions_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.ClosingDate).HasColumnName("closing_date");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.CompressionPosition)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("compression_position");
            entity.Property(e => e.Compressionable).HasColumnName("compressionable");
            entity.Property(e => e.ConceptId).HasColumnName("concept_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DepositAffiliateId).HasColumnName("deposit_affiliate_id");
            entity.Property(e => e.Gif).HasColumnName("gif");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SalesPrice).HasColumnName("sales_price");
            entity.Property(e => e.SubtractBinary).HasColumnName("subtract_binary");
            entity.Property(e => e.Tax).HasColumnName("tax");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17322_pk__credits__3214ec0742e2ff69");

            entity.ToTable("credits", "wallet_service");

            entity.HasIndex(e => e.AffiliateId, "idx_17322_index_affiliate_id");

            entity.HasIndex(e => e.ConceptId, "idx_17322_index_concept_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('credits_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.ConceptId).HasColumnName("concept_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit1).HasColumnName("credit");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Islr).HasColumnName("islr");
            entity.Property(e => e.Iva).HasColumnName("iva");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.Request).HasColumnName("request");
            entity.Property(e => e.RequestDenied).HasColumnName("request_denied");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Credits)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("fk_credits_invoices");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17343_invoices_pk");

            entity.ToTable("invoices", "wallet_service");

            entity.HasIndex(e => e.AffiliateId, "idx_17343_index_affiliate_id");

            entity.HasIndex(e => e.CancellationDate, "idx_17343_indexcancelationdate");

            entity.HasIndex(e => e.BrandId, "idx_17343_ix_invoices_brandid");

            entity.HasIndex(e => e.ReceiptNumber, "idx_17343_ix_invoices_receiptnumber");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('invoices_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.BtcAddress).HasColumnName("btc_address");
            entity.Property(e => e.CancellationDate).HasColumnName("cancellation_date");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DepositDate).HasColumnName("deposit_date");
            entity.Property(e => e.InvoiceAddress).HasColumnName("invoice_address");
            entity.Property(e => e.InvoiceData).HasColumnName("invoice_data");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.PurchaseOrderId)
                .HasDefaultValueSql("0")
                .HasColumnName("purchase_order_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReceiptNumber).HasColumnName("receipt_number");
            entity.Property(e => e.Recurring).HasColumnName("recurring");
            entity.Property(e => e.SecretKey).HasColumnName("secret_key");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.State)
                .HasDefaultValueSql("true")
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.TotalCommissionable).HasColumnName("total_commissionable");
            entity.Property(e => e.TotalInvoice).HasColumnName("total_invoice");
            entity.Property(e => e.TotalInvoiceBtc)
                .HasDefaultValueSql("0.00000000")
                .HasColumnName("total_invoice_btc");
            entity.Property(e => e.TotalPoints).HasColumnName("total_points");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.IncludeInCommissionCalculation).HasColumnName("include_in_commission_calculation").HasDefaultValue(true);

            entity.HasOne(d => d.Brand).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoices_brand");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<InvoicePack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17329_pk__invoicep__3214ec07a340ee67");

            entity.ToTable("invoice_packs", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('invoicepacks_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.CountDays).HasColumnName("count_days");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoicePacks)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicepacks_invoices");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<InvoicePacksDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17336_pk__invoicep__3214ec07c1135c81");

            entity.ToTable("invoice_packs_details", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('invoicepacksdetails_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoicePackId).HasColumnName("invoice_pack_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.InvoicePack).WithMany(p => p.InvoicePacksDetails)
                .HasForeignKey(d => d.InvoicePackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicepacksdetails_invoicepacks");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<InvoicesDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17355_invoicesdetails_pk");

            entity.ToTable("invoices_details", "wallet_service");

            entity.HasIndex(e => e.PaymentGroupId, "idx_17355_indexpaymentgroup");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('invoicesdetails_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AccumminPurchase).HasColumnName("accummin_purchase");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.BinaryPoints)
                .HasDefaultValueSql("0.00")
                .HasColumnName("binary_points");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CombinationId)
                .HasDefaultValueSql("0")
                .HasColumnName("combination_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DailyPercentage).HasColumnName("daily_percentage");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DaysToPayQuantity)
                .HasDefaultValueSql("0")
                .HasColumnName("days_to_pay_quantity");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.PaymentGroupId).HasColumnName("payment_group_id");
            entity.Property(e => e.ProductCommissionable).HasColumnName("product_commissionable");
            entity.Property(e => e.ProductDiscount)
                .HasDefaultValueSql("0.00")
                .HasColumnName("product_discount");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductIva).HasColumnName("product_iva");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProductPack)
                .HasDefaultValueSql("false")
                .HasColumnName("product_pack");
            entity.Property(e => e.ProductPoints).HasColumnName("product_points");
            entity.Property(e => e.ProductPrice).HasColumnName("product_price");
            entity.Property(e => e.ProductPriceBtc)
                .HasDefaultValueSql("0.00")
                .HasColumnName("product_price_btc");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.ProductStart).HasColumnName("product_start");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.WaitingDays).HasColumnName("waiting_days");

            entity.HasOne(d => d.Brand).WithMany(p => p.InvoicesDetails)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicesdetails_brand");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoicesDetails)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invoicesdetails_invoices");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<LeaderBoardModel5>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17369_pk__leaderbo__3214ec076149865f");

            entity.ToTable("leader_board_model5", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('leaderboardmodel5_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.GradingId).HasColumnName("grading_id");
            entity.Property(e => e.GradingPosition).HasColumnName("grading_position");
            entity.Property(e => e.MatrixPosition).HasColumnName("matrix_position");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<LeaderBoardModel6>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17376_pk__leaderbo__3214ec07c4bba265");

            entity.ToTable("leader_board_model6", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('leaderboardmodel6_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.GradingId).HasColumnName("grading_id");
            entity.Property(e => e.GradingPosition).HasColumnName("grading_position");
            entity.Property(e => e.MatrixPosition).HasColumnName("matrix_position");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<ModelConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17383_pk__ecopoolc__3214ec07f574c1f0");

            entity.ToTable("model_configuration", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('modelconfiguration_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Case).HasColumnName("case");
            entity.Property(e => e.CompanyPercentage).HasColumnName("company_percentage");
            entity.Property(e => e.CompanyPercentageLevels).HasColumnName("company_percentage_levels");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DateInit).HasColumnName("date_init");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.MaxGainLimit).HasColumnName("max_gain_limit");
            entity.Property(e => e.ModelPercentage).HasColumnName("model_percentage");
            entity.Property(e => e.ModelType)
                .HasDefaultValueSql("'Model_2'::text")
                .HasColumnName("model_type");
            entity.Property(e => e.Processed).HasColumnName("processed");
            entity.Property(e => e.Totals).HasColumnName("totals");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ModelConfigurationLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17391_pk__ecopooll__3214ec07860bbf9a");

            entity.ToTable("model_configuration_levels", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('modelconfigurationlevels_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.EcopoolConfigurationId).HasColumnName("ecopool_configuration_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.EcopoolConfiguration).WithMany(p => p.ModelConfigurationLevels)
                .HasForeignKey(d => d.EcopoolConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ecopoollevels_ecopoolconfiguration");
        });

        modelBuilder.Entity<ModelFourStatistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17398_pk__modelfou__3214ec07100763fc");

            entity.ToTable("model_four_statistics", "wallet_service");

            entity.HasIndex(e => e.AffiliateId, "idx_17398_affiliateidindex");

            entity.HasIndex(e => e.AffiliateNetworkId, "idx_17398_affiliatenetworkidindex");

            entity.HasIndex(e => e.InvoiceId, "idx_17398_invoiceidindex");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('modelfourstatistics_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateNetworkId).HasColumnName("affiliate_network_id");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreditLeft)
                .HasDefaultValueSql("0.00")
                .HasColumnName("credit_left");
            entity.Property(e => e.CreditRight)
                .HasDefaultValueSql("0.00")
                .HasColumnName("credit_right");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DebitLeft)
                .HasDefaultValueSql("0.00")
                .HasColumnName("debit_left");
            entity.Property(e => e.DebitRight)
                .HasDefaultValueSql("0.00")
                .HasColumnName("debit_right");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
        });

        modelBuilder.Entity<NetworkPurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17410_pk__networkp__3214ec07876eb63c");

            entity.ToTable("network_purchases", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('networkpurchases_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.CommisionableAmount).HasColumnName("commisionable_amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Origin)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("origin");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ResultsModel1a>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17418_pk__resultsm__3214ec0785066783");

            entity.ToTable("results_model_1a", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel1a_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PeriodPool).HasColumnName("period_pool");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProfitDistributedLevels).HasColumnName("profit_distributed_levels");
            entity.Property(e => e.TotalPercentage).HasColumnName("total_percentage");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
        });

        modelBuilder.Entity<ResultsModel1aLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17425_pk__resultsm__3214ec072532a76e");

            entity.ToTable("results_model_1a_levels", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel1alevels_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BinarySide).HasColumnName("binary_side");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PercentageLevel).HasColumnName("percentage_level");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ResultsModel1aId).HasColumnName("results_model_1a_id");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");

            entity.HasOne(d => d.ResultsModel1a).WithMany(p => p.ResultsModel1aLevels)
                .HasForeignKey(d => d.ResultsModel1aId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resultsmodel1alevels_resultsmodel1a");
        });

        modelBuilder.Entity<ResultsModel1b>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17432_pk__resultsm__3214ec0736e530fa");

            entity.ToTable("results_model_1b", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel1b_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PeriodPool).HasColumnName("period_pool");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProfitDistributedLevels).HasColumnName("profit_distributed_levels");
            entity.Property(e => e.TotalPercentage).HasColumnName("total_percentage");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
        });

        modelBuilder.Entity<ResultsModel1bLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17439_pk__resultsm__3214ec0709cc78ca");

            entity.ToTable("results_model_1b_levels", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel1blevels_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BinarySide).HasColumnName("binary_side");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PercentageLevel).HasColumnName("percentage_level");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ResultsModel1bId).HasColumnName("results_model_1b_id");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");

            entity.HasOne(d => d.ResultsModel1b).WithMany(p => p.ResultsModel1bLevels)
                .HasForeignKey(d => d.ResultsModel1bId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resultsmodel1blevels_resultsmodel1b");
        });

        modelBuilder.Entity<ResultsModel2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17446_pk__resultse__3214ec07c6ce7aa5");

            entity.ToTable("results_model2", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel2_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.BasePack).HasColumnName("base_pack");
            entity.Property(e => e.CasePool).HasColumnName("case_pool");
            entity.Property(e => e.CompanyAmount).HasColumnName("company_amount");
            entity.Property(e => e.CompanyPercentage).HasColumnName("company_percentage");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.DailyPercentage).HasColumnName("daily_percentage");
            entity.Property(e => e.DaysAmount).HasColumnName("days_amount");
            entity.Property(e => e.DeductionAmount).HasColumnName("deduction_amount");
            entity.Property(e => e.EcopoolConfigurationId).HasColumnName("ecopool_configuration_id");
            entity.Property(e => e.LastDayDate).HasColumnName("last_day_date");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.PeriodPool).HasColumnName("period_pool");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProfitDistributedLevels).HasColumnName("profit_distributed_levels");
            entity.Property(e => e.TotalPercentage).HasColumnName("total_percentage");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");

            entity.HasOne(d => d.ModelConfiguration).WithMany(p => p.ResultsModel2s)
                .HasForeignKey(d => d.EcopoolConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resultsecopool_resultecopoollevels");
        });

        modelBuilder.Entity<ResultsModel2Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17453_pk__resultec__3214ec07db8f1fe4");

            entity.ToTable("results_model2_levels", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel2levels_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BinarySide).HasColumnName("binary_side");
            entity.Property(e => e.CommisionAmount).HasColumnName("commision_amount");
            entity.Property(e => e.CompanyAmountLevel).HasColumnName("company_amount_level");
            entity.Property(e => e.CompanyPercentageLevel).HasColumnName("company_percentage_level");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PercentageLevel).HasColumnName("percentage_level");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ResultEcopoolId).HasColumnName("result_ecopool_id");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");

            entity.HasOne(d => d.ResultEcopool).WithMany(p => p.ResultsModel2Levels)
                .HasForeignKey(d => d.ResultEcopoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resultecopoollevels_resultsecopool");
        });

        modelBuilder.Entity<ResultsModel3>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17460_pk__resultsm__3214ec0732a3cff2");

            entity.ToTable("results_model3", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel3_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PeriodPool).HasColumnName("period_pool");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ProductExternalId).HasColumnName("product_external_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProfitDistributedLevels).HasColumnName("profit_distributed_levels");
            entity.Property(e => e.TotalPercentage).HasColumnName("total_percentage");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
        });

        modelBuilder.Entity<ResultsModel3Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17467_pk__resultsm__3214ec0750e720a6");

            entity.ToTable("results_model3_levels", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('resultsmodel3levels_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateName).HasColumnName("affiliate_name");
            entity.Property(e => e.BinarySide).HasColumnName("binary_side");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");
            entity.Property(e => e.PercentageLevel).HasColumnName("percentage_level");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.ResultsModel3Id).HasColumnName("results_model3_id");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");

            entity.HasOne(d => d.ResultsModel3).WithMany(p => p.ResultsModel3Levels)
                .HasForeignKey(d => d.ResultsModel3Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resultsmodeltwolevels_resultsmodeltwo");
        });

        modelBuilder.Entity<Sysdiagram>(entity =>
        {
            entity.HasKey(e => e.DiagramId).HasName("idx_17474_pk__sysdiagr__c2b05b613c82a1cb");

            entity.ToTable("sysdiagrams", "wallet_service");

            entity.HasIndex(e => new { e.PrincipalId, e.Name }, "idx_17474_uk_principal_name").IsUnique();

            entity.Property(e => e.DiagramId)
                .HasDefaultValueSql("nextval('sysdiagrams_diagram_id_seq'::regclass)")
                .HasColumnName("diagram_id");
            entity.Property(e => e.Definition).HasColumnName("definition");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PrincipalId).HasColumnName("principal_id");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("idx_17480_pk__transact__20266d0b749ec053");

            entity.ToTable("transaction_types", "wallet_service");

            entity.HasIndex(e => e.TypeName, "idx_17480_uq__transact__d4e7dfa86b2d62ba").IsUnique();

            entity.Property(e => e.TransactionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_type_id");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<VolumePurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17486_pk__volumepu__3214ec07eddff4b4");

            entity.ToTable("volume_purchases", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('volumepurchases_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateIdGeneric).HasColumnName("affiliate_id_generic");
            entity.Property(e => e.Compression)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreditD)
                .HasDefaultValueSql("0.00")
                .HasColumnName("credit_d");
            entity.Property(e => e.CreditI)
                .HasDefaultValueSql("0.00")
                .HasColumnName("credit_i");
            entity.Property(e => e.DebitD)
                .HasDefaultValueSql("0.00")
                .HasColumnName("debit_d");
            entity.Property(e => e.DebitI)
                .HasDefaultValueSql("0.00")
                .HasColumnName("debit_i");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17498_wallets_pk");

            entity.ToTable("wallets", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('wallets_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AdminUserName).HasColumnName("admin_user_name");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Compression)
                .HasDefaultValueSql("false")
                .HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.ConceptType).HasColumnName("concept_type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit)
                .HasDefaultValueSql("0.0")
                .HasColumnName("credit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit)
                .HasDefaultValueSql("0.0")
                .HasColumnName("debit");
            entity.Property(e => e.Deferred).HasColumnName("deferred");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Detail).HasColumnName("detail");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("true")
                .HasColumnName("status");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_wallets_brand");

            entity
                .HasIndex(e => new { e.AffiliateId, e.Concept, e.Detail })
                .HasDatabaseName("ux_wallet_unique") 
                .IsUnique();
            
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17509_walletshistories_pk");

            entity.ToTable("wallets_histories", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletshistories_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit)
                .HasDefaultValueSql("0.00")
                .HasColumnName("credit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit)
                .HasDefaultValueSql("0.00")
                .HasColumnName("debit");
            entity.Property(e => e.Deferred).HasColumnName("deferred");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsModel1a>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17518_pk__walletsm__3214ec07f4560047");

            entity.ToTable("wallets_model_1a", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsmodel1a_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AdminUserName).HasColumnName("admin_user_name");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.ConceptType).HasColumnName("concept_type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.Deferred).HasColumnName("deferred");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Detail).HasColumnName("detail");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsModel1b>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17525_pk__walletsm__3214ec07a2279a36");

            entity.ToTable("wallets_model_1b", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsmodel1b_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AdminUserName).HasColumnName("admin_user_name");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.Compression).HasColumnName("compression");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.ConceptType).HasColumnName("concept_type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.Deferred).HasColumnName("deferred");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Detail).HasColumnName("detail");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17532_walletsperiods_pk");

            entity.ToTable("wallets_periods", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsperiods_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17537_walletsrequests_pk");

            entity.ToTable("wallets_requests", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsrequests_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AdminUserName).HasColumnName("admin_user_name");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.AttentionDate).HasColumnName("attention_date");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
            entity.Property(e => e.PaymentAffiliateId).HasColumnName("payment_affiliate_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Brand).WithMany(p => p.WalletsRequests)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_walletsrequests_brand");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsRetentionsConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17544_walletsretentionsconfigs_pk");

            entity.ToTable("wallets_retentions_configs", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsretentionsconfigs_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DisableDate).HasColumnName("disable_date");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.WithdrawalFrom).HasColumnName("withdrawal_from");
            entity.Property(e => e.WithdrawalTo).HasColumnName("withdrawal_to");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsServiceModel1a>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17552_pk__walletss__3214ec07c324d429");

            entity.ToTable("wallets_service_model_1a", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsservicemodel1a_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsServiceModel1b>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17559_pk__walletss__3214ec07b63492f5");

            entity.ToTable("wallets_service_model_1b", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsservicemodel1b_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsServiceModel2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17566_pk__walletss__3214ec0757c66f92");

            entity.ToTable("wallets_service_model2", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletsservicemodel2_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Concept).HasColumnName("concept");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Debit).HasColumnName("debit");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.WalletsServiceModel2s)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_walletsservicemodel2_brand");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsWait>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17573_walletswaits_pk");

            entity.ToTable("wallets_waits", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletswaits_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.Attended)
                .HasDefaultValueSql("false")
                .HasColumnName("attended");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DepositDate).HasColumnName("deposit_date");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("false")
                .HasColumnName("status");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<WalletsWithdrawal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_17582_walletswithdrawals_pk");

            entity.ToTable("wallets_withdrawals", "wallet_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('walletswithdrawals_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.AdminObservation).HasColumnName("admin_observation");
            entity.Property(e => e.AffiliateId).HasColumnName("affiliate_id");
            entity.Property(e => e.AffiliateUserName).HasColumnName("affiliate_user_name");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.IsProcessed).HasColumnName("is_processed");
            entity.Property(e => e.Observation).HasColumnName("observation");
            entity.Property(e => e.ResponseDate).HasColumnName("response_date");
            entity.Property(e => e.RetentionPercentage).HasColumnName("retention_percentage");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<InvoicesSpResponse>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("handle_debit_transaction", "wallet_service");
        });

        modelBuilder.Entity<MatrixQualification>(entity =>
        {
            entity.ToTable("matrix_qualification", "wallet_service");

            entity.HasKey(e => e.QualificationId);
            entity.Property(e => e.QualificationId).HasColumnName("qualification_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MatrixType).HasColumnName("matrix_type");
            entity.Property(e => e.TotalEarnings).HasColumnName("total_earnings");
            entity.Property(e => e.WithdrawnAmount).HasColumnName("withdrawn_amount");
            entity.Property(e => e.AvailableBalance).HasColumnName("available_balance");
            entity.Property(e => e.IsQualified).HasColumnName("is_qualified");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.QualificationCount).HasColumnName("qualification_count");
            entity.Property(e => e.LastQualificationWithdrawnAmount).HasColumnName("last_qualification_withdrawn_amount");
            entity.Property(e => e.LastQualificationTotalEarnings).HasColumnName("last_qualification_total_earnings");
            entity.Property(e => e.LastQualificationDate).HasColumnName("last_qualification_date");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<MatrixEarning>(entity =>
        {
            entity.ToTable("matrix_earnings", "wallet_service");

            entity.HasKey(e => e.EarningId);
            entity.Property(e => e.EarningId).HasColumnName("earning_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MatrixType).HasColumnName("matrix_type");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.SourceUserId).HasColumnName("source_user_id");
            entity.Property(e => e.EarningType).HasColumnName("earning_type").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<PurchasesPerMonth>().HasNoKey();
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}