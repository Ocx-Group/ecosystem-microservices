using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Context;

public partial class AccountServiceDbContext : DbContext
{
    private const string ColCreatedAt        = "created_at";
    private const string ColUpdatedAt        = "updated_at";
    private const string ColDeletedAt        = "deleted_at";
    private const string ColStatus           = "status";
    private const string ColBrandId          = "brand_id";
    private const string ColUsername         = "username";
    private const string ColAddress          = "address";
    private const string ColAffiliateId      = "affiliate_id";
    private const string SchemaName          = "account_service";
    private const string SqlCurrentTimestamp = "CURRENT_TIMESTAMP";

    public AccountServiceDbContext() { }

    public AccountServiceDbContext(DbContextOptions<AccountServiceDbContext> options)
        : base(options) { }

    public virtual DbSet<AffiliatesAddress> AffiliatesAddresses { get; set; }
    public virtual DbSet<AffiliatesAddressCoinpayRegister> AffiliatesAddressCoinpayRegisters { get; set; }
    public virtual DbSet<AffiliatesBtc> AffiliatesBtcs { get; set; }
    public virtual DbSet<AffiliatesCountRegister> AffiliatesCountRegisters { get; set; }
    public virtual DbSet<ApiClient> ApiClients { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<LeaderBoardModel4> LeaderBoardModel4s { get; set; }
    public virtual DbSet<LeaderBoardModel5> LeaderBoardModel5s { get; set; }
    public virtual DbSet<LeaderBoardModel6> LeaderBoardModel6s { get; set; }
    public virtual DbSet<LoginMovement> LoginMovements { get; set; }
    public virtual DbSet<MenuConfiguration> MenuConfigurations { get; set; }
    public virtual DbSet<Network> Networks { get; set; }
    public virtual DbSet<Privilege> Privileges { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketCategory> TicketCategories { get; set; }
    public virtual DbSet<TicketImage> TicketImages { get; set; }
    public virtual DbSet<TicketMessage> TicketMessages { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UsersAffiliate> UsersAffiliates { get; set; }
    public virtual DbSet<AffiliatePersonalNetwork> AffiliatePersonalNetwork { get; set; }
    public virtual DbSet<MatrixPosition> MatrixPositions { get; set; }
    public virtual DbSet<MatrixCycle> MatrixCycles { get; set; }
    public virtual DbSet<MasterPassword> MasterPasswords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AffiliatesAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16562_pk__affiliat__3214ec070b0a5926");
            entity.ToTable("affiliates_address", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.affiliatesaddress_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName(ColAddress);
            entity.Property(e => e.AddressLine2).HasColumnName("address_line2");
            entity.Property(e => e.AddressName).HasColumnName("address_name");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Company).HasColumnName("company");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CountryName).HasColumnName("country_name");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FiscalIdentification).HasColumnName("fiscal_identification");
            entity.Property(e => e.IvaNumber).HasColumnName("iva_number");
            entity.Property(e => e.LandlinePhone).HasColumnName("landline_phone");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MobilePhone).HasColumnName("mobile_phone");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Other).HasColumnName("other");
            entity.Property(e => e.PostalCode).HasColumnName("postal_code");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.StateName).HasColumnName("state_name");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.Affiliate).WithMany(p => p.AffiliatesAddresses).HasForeignKey(d => d.AffiliateId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_affiliatesaddress_usersaffiliate");
            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.AffiliatesAddresses).HasForeignKey(d => d.Country).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_affiliatesaddress_countries");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<AffiliatesAddressCoinpayRegister>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16569_pk__affiliat__3214ec07fd6d49f7");
            entity.ToTable("affiliates_address_coinpay_register", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.affiliatesaddresscoinpayregister_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName(ColAddress);
            entity.Property(e => e.ExternalReferenceId).HasColumnName("external_reference_id");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
        });

        modelBuilder.Entity<AffiliatesBtc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16576_pk__affiliat__3213e83fc8b2c814");
            entity.ToTable("affiliates_btc", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.affiliatesbtc_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName(ColAddress);
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.NetworkId).HasColumnName("network_id");
            entity.Property(e => e.Status).HasDefaultValueSql("'0'::smallint").HasColumnName(ColStatus);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.Affiliate).WithMany(p => p.AffiliatesBtcs).HasForeignKey(d => d.AffiliateId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_affiliatesbtc_usersaffiliate");
            entity.HasOne(d => d.Brand).WithMany(p => p.AffiliatesBtcs).HasForeignKey(d => d.BrandId).HasConstraintName("fk_affiliatesbtc_brand");
            entity.HasOne(d => d.Network).WithMany(p => p.AffiliatesBtcs).HasForeignKey(d => d.NetworkId).HasConstraintName("fk_affiliatesbtc_network");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<AffiliatesCountRegister>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16584_pk__affiliat__3214ec07c5b5086e");
            entity.ToTable("affiliates_count_register", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.affiliatescountregister_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.Count).HasDefaultValueSql("0").HasColumnName("count");
            entity.Property(e => e.CountRedBinaria).HasDefaultValueSql("0").HasColumnName("count_red_binaria");
            entity.Property(e => e.CountRedForzada).HasDefaultValueSql("0").HasColumnName("count_red_forzada");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.HasOne(d => d.Affiliate).WithMany(p => p.AffiliatesCountRegisters).HasForeignKey(d => d.AffiliateId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_afiliadoscountregister_usersaffiliate");
        });

        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16592_pk__apiclien__3214ec07a968a2fb");
            entity.ToTable("api_client", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.apiclient_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16599_pk__brand__3214ec074491a225");
            entity.ToTable("brand", SchemaName);
            entity.HasIndex(e => e.IsActive, "idx_16599_ix_brand_isactive").HasFilter("(is_active = true)");
            entity.HasIndex(e => e.Name, "idx_16599_ix_brand_name");
            entity.HasIndex(e => e.SecretKey, "idx_16599_uq_brand_secretkey").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("true").HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SecretKey).HasColumnName("secret_key");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16607_pk__countrie__3214ec077a436e70");
            entity.ToTable("countries", SchemaName);
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Capital).HasColumnName("capital");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.CurrencyName).HasColumnName("currency_name");
            entity.Property(e => e.CurrencySymbol).HasColumnName("currency_symbol");
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Iso2).HasColumnName("iso2");
            entity.Property(e => e.Iso3).HasColumnName("iso3");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Native).HasColumnName("native");
            entity.Property(e => e.NumericCode).HasColumnName("numeric_code");
            entity.Property(e => e.PhoneCode).HasColumnName("phone_code");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Subregion).HasColumnName("subregion");
            entity.Property(e => e.Tld).HasColumnName("tld");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<LeaderBoardModel4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16613_pk__leaderbo__3214ec07c194d079");
            entity.ToTable("leader_board_model4", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.leaderboardmodel4_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.FatherModel4).HasColumnName("father_model4");
            entity.Property(e => e.GradingId).HasColumnName("grading_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PositionX).HasColumnName("position_x");
            entity.Property(e => e.PositionY).HasColumnName("position_y");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
            entity.Property(e => e.Username).HasColumnName(ColUsername);
        });

        modelBuilder.Entity<LeaderBoardModel5>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16620_pk__leaderbo__3214ec070fdaf05c");
            entity.ToTable("leader_board_model5", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.leaderboardmodel5_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.FatherModel5).HasColumnName("father_model5");
            entity.Property(e => e.GradingId).HasColumnName("grading_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PositionX).HasColumnName("position_x");
            entity.Property(e => e.PositionY).HasColumnName("position_y");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
            entity.Property(e => e.Username).HasColumnName(ColUsername);
        });

        modelBuilder.Entity<LeaderBoardModel6>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16627_pk__leaderbo__3214ec0790be4fa4");
            entity.ToTable("leader_board_model6", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.leaderboardmodel6_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.FatherModel6).HasColumnName("father_model6");
            entity.Property(e => e.GradingId).HasColumnName("grading_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.PositionX).HasColumnName("position_x");
            entity.Property(e => e.PositionY).HasColumnName("position_y");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
            entity.Property(e => e.Username).HasColumnName(ColUsername);
        });

        modelBuilder.Entity<LoginMovement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16634_pk__loginmov__3214ec07fa96b08c");
            entity.ToTable("login_movements", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.loginmovements_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.BrandId).HasDefaultValueSql("1").HasColumnName(ColBrandId);
            entity.Property(e => e.BrowserInfo).HasColumnName("browser_info");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            entity.Property(e => e.OperatingSystem).HasColumnName("operating_system");
            entity.Property(e => e.Status).HasColumnName(ColStatus);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.Brand).WithMany(p => p.LoginMovements).HasForeignKey(d => d.BrandId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_loginmovements_brand");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<MenuConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16642_pk__menuconf__3214ec0797f36589");
            entity.ToTable("menu_configurations", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.menuconfigurations_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.MenuName).HasColumnName("menu_name");
            entity.Property(e => e.PageName).HasColumnName("page_name");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Network>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16649_pk__network__3214ec07e5c99226");
            entity.ToTable("network", SchemaName);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Status).HasDefaultValueSql("true").HasColumnName(ColStatus);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16659_pk__privileg__3214ec071eb08342");
            entity.ToTable("privileges", SchemaName);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CanCreate).HasColumnName("can_create");
            entity.Property(e => e.CanDelete).HasColumnName("can_delete");
            entity.Property(e => e.CanEdit).HasColumnName("can_edit");
            entity.Property(e => e.CanRead).HasColumnName("can_read");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.MenuConfigurationId).HasColumnName("menu_configuration_id");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.MenuConfiguration).WithMany(p => p.Privileges).HasForeignKey(d => d.MenuConfigurationId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_privileges_menuconfigurations");
            entity.HasOne(d => d.Rol).WithMany(p => p.Privileges).HasForeignKey(d => d.RolId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_privileges_roles");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16668_pk__roles__3214ec0726ebcef8");
            entity.ToTable("roles", SchemaName);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16703_pk__tickets__3214ec07684bd550");
            entity.ToTable("tickets", SchemaName);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AffiliateId).HasColumnName(ColAffiliateId);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.Status).IsRequired().HasDefaultValueSql("true").HasColumnName(ColStatus);
            entity.Property(e => e.Subject).HasColumnName("subject");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.Brand).WithMany(p => p.Tickets).HasForeignKey(d => d.BrandId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_brand");
            entity.HasOne(d => d.Category).WithMany(p => p.Tickets).HasForeignKey(d => d.CategoryId).HasConstraintName("fk__tickets__categor__19dfd96b");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16675_pk__ticketca__3214ec072ff3ef2d");
            entity.ToTable("ticket_categories", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.ticketcategories_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColUpdatedAt);
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<TicketImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16684_pk__ticketim__3214ec071fb80b2e");
            entity.ToTable("ticket_images", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.ticketimages_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.ImagePath).HasColumnName("image_path");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColUpdatedAt);
            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketImages).HasForeignKey(d => d.TicketId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk__ticketima__ticke__540c7b00");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<TicketMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16693_pk__ticketme__3214ec072656a94d");
            entity.ToTable("ticket_messages", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.ticketmessages_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.MessageContent).HasColumnName("message_content");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketMessages).HasForeignKey(d => d.TicketId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk__ticketmes__ticke__5ab9788f");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16714_pk__users__3214ec07ff55f305");
            entity.ToTable("users", SchemaName);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName(ColAddress);
            entity.Property(e => e.BrandId).HasDefaultValueSql("1").HasColumnName(ColBrandId);
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.ImageProfileUrl).HasColumnName("image_profile_url");
            entity.Property(e => e.LastActivity).HasColumnName("last_activity");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Observation).HasColumnName("observation");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.SecretAnswer).HasColumnName("secret_answer");
            entity.Property(e => e.SecretQuestion).HasColumnName("secret_question");
            entity.Property(e => e.Status).IsRequired().HasDefaultValueSql("true").HasColumnName(ColStatus);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.Username).HasColumnName(ColUsername);
            entity.HasOne(d => d.Brand).WithMany(p => p.Users).HasForeignKey(d => d.BrandId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_users_brand");
            entity.HasOne(d => d.Rol).WithMany(p => p.Users).HasForeignKey(d => d.RolId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_users_roles");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<UsersAffiliate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("idx_16723_pk__usersaff__3214ec072491ddfd");
            entity.ToTable("users_affiliate", SchemaName);
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('account_service.usersaffiliate_id_seq'::regclass)").HasColumnName("id");
            entity.Property(e => e.ActivationDate).HasColumnName("activation_date");
            entity.Property(e => e.ActiveCap).HasDefaultValueSql("'0'::smallint").HasColumnName("active_cap");
            entity.Property(e => e.Address).HasColumnName(ColAddress);
            entity.Property(e => e.AffiliateMode).HasColumnName("affiliate_mode");
            entity.Property(e => e.AffiliateType).HasColumnName("affiliate_type");
            entity.Property(e => e.Attempts).HasDefaultValueSql("0").HasColumnName("attempts");
            entity.Property(e => e.AuthorizationDate).HasColumnName("authorization_date");
            entity.Property(e => e.BeneficiaryName).HasColumnName("beneficiary_name").HasMaxLength(70);
            entity.Property(e => e.BeneficiaryEmail).HasColumnName("beneficiary_email").HasMaxLength(100);
            entity.Property(e => e.BeneficiaryPhone).HasColumnName("beneficiary_phone").HasMaxLength(40);
            entity.Property(e => e.BinaryMatrixSide).HasColumnName("binary_matrix_side");
            entity.Property(e => e.BinarySponsor).HasColumnName("binary_sponsor");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.BrandId).HasDefaultValueSql("1").HasColumnName(ColBrandId);
            entity.Property(e => e.CardIdAuthorization).HasColumnName("card_id_authorization");
            entity.Property(e => e.CardIdMessage).HasColumnName("card_id_message");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.CoinpaymentsCap).HasColumnName("coinpayments_cap");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.EmailVerification).HasColumnName("email_verification");
            entity.Property(e => e.ExternalGradingId).HasColumnName("external_grading_id");
            entity.Property(e => e.ExternalGradingIdBefore).HasColumnName("external_grading_id_before");
            entity.Property(e => e.ExternalProductId).HasColumnName("external_product_id");
            entity.Property(e => e.Father).HasColumnName("father");
            entity.Property(e => e.GoogleAuthCode).HasColumnName("google_auth_code");
            entity.Property(e => e.Identification).HasColumnName("identification");
            entity.Property(e => e.IdentificationType).HasColumnName("identification_type");
            entity.Property(e => e.ImagePathId).HasColumnName("image_path_id");
            entity.Property(e => e.ImageProfileUrl).HasColumnName("image_profile_url");
            entity.Property(e => e.IsBinaryEvaluated).HasColumnName("is_binary_evaluated");
            entity.Property(e => e.IsForcedComplete).HasColumnName("is_forced_complete");
            entity.Property(e => e.IsGoogleAuth).HasDefaultValueSql("false").HasColumnName("is_google_auth");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.LegalAuthorizedFirst).HasColumnName("legal_authorized_first");
            entity.Property(e => e.LegalAuthorizedSecond).HasColumnName("legal_authorized_second");
            entity.Property(e => e.MessageAlert).HasDefaultValueSql("'0'::smallint").HasColumnName("message_alert");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PasswordTemp).HasColumnName("password_temp");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.PrivateKey).HasColumnName("private_key");
            entity.Property(e => e.SecretAnswer).HasColumnName("secret_answer");
            entity.Property(e => e.SecretQuestion).HasColumnName("secret_question");
            entity.Property(e => e.SecurityPin).HasColumnName("security_pin");
            entity.Property(e => e.Side).HasColumnName("side");
            entity.Property(e => e.Sponsor).HasColumnName("sponsor");
            entity.Property(e => e.StatePlace).HasColumnName("state_place");
            entity.Property(e => e.Status).HasColumnName(ColStatus);
            entity.Property(e => e.StatusActivation).HasColumnName("status_activation");
            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.TermsConditions).HasColumnName("terms_conditions");
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.Usepin).HasColumnName("usepin");
            entity.Property(e => e.Username).HasColumnName(ColUsername);
            entity.Property(e => e.VerificationCode).HasColumnName("verification_code");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");
            entity.HasOne(d => d.Brand).WithMany(p => p.UsersAffiliates).HasForeignKey(d => d.BrandId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_usersaffiliate_brand");
            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.UsersAffiliates).HasForeignKey(d => d.Country).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_usersaffiliate_country");
            entity.HasQueryFilter(x => !x.DeletedAt.HasValue);
        });

        modelBuilder.Entity<UniLevelFamilyTree>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("UniLevelFamilyTree");
            entity.Property(u => u.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<ModelsFamilyTree>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("ModelsFamilyTree");
            entity.Property(u => u.UserName).HasColumnName("user_name");
        });

        modelBuilder.Entity<AffiliatePersonalNetwork>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasColumnType("bigint");
        });

        modelBuilder.Entity<MessageDetails>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasColumnType("bigint");
        });

        modelBuilder.Entity<MatrixPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("matrix_positions_pkey");
            entity.ToTable("matrix_positions", SchemaName);
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MatrixType).HasColumnName("matrix_type");
            entity.Property(e => e.ParentPositionId).HasColumnName("parent_position_id");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.HasOne(d => d.User).WithMany(p => p.MatrixPositions).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("matrix_positions_user_id_fkey");
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<MatrixTree>().HasNoKey();

        modelBuilder.Entity<MatrixCycle>(entity =>
        {
            entity.ToTable("matrix_cycles", SchemaName);
            entity.HasKey(e => e.CycleId);
            entity.Property(e => e.CycleId).HasColumnName("cycle_id");
            entity.Property(e => e.MatrixType).HasColumnName("matrix_type");
            entity.Property(e => e.InitiatorUserId).HasColumnName("initiator_user_id");
            entity.Property(e => e.TotalPositions).HasColumnName("total_positions");
            entity.Property(e => e.MaxPositions).HasColumnName("max_positions");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.RewardPaid).HasColumnName("reward_paid");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<MasterPassword>(entity =>
        {
            entity.ToTable("master_password", SchemaName);
            entity.HasKey(e => e.Id).HasName("master_password_pkey");
            entity.HasKey(e => new { e.Id, e.BrandId }).HasName("master_password_id_brand_id_key");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).IsRequired().HasColumnName(ColBrandId);
            entity.Property(e => e.Password).IsRequired().HasColumnName("password");
            entity.Property(e => e.Algorithm).HasColumnName("algorithm");
            entity.Property(e => e.CreatedAt).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
        
        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
