using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Context;

public partial class InventoryServiceDbContext : DbContext
{
    private const string ColCreatedAt = "created_at";
    private const string ColUpdatedAt = "updated_at";
    private const string ColDeletedAt = "deleted_at";
    private const string ColBrandId = "brand_id";
    private const string SchemaName = "inventory_service";
    private const string SqlCurrentTimestamp = "CURRENT_TIMESTAMP";

    public InventoryServiceDbContext() { }

    public InventoryServiceDbContext(DbContextOptions<InventoryServiceDbContext> options)
        : base(options) { }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductsCategory> ProductsCategories { get; set; }
    public virtual DbSet<ProductsInventory> ProductsInventories { get; set; }
    public virtual DbSet<ProductsAttribute> ProductsAttributes { get; set; }
    public virtual DbSet<ProductsAttributesValue> ProductsAttributesValues { get; set; }
    public virtual DbSet<ProductsDiscount> ProductsDiscounts { get; set; }
    public virtual DbSet<ProductsBanner> ProductsBanners { get; set; }
    public virtual DbSet<ProductsCombination> ProductsCombinations { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<ApiClient> ApiClients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.SalePrice).HasColumnName("sale_price");
            entity.Property(e => e.CommissionableValue).HasColumnName("commissionable_value");
            entity.Property(e => e.BinaryPoints).HasColumnName("binary_points");
            entity.Property(e => e.ValuePoints).HasColumnName("value_points");
            entity.Property(e => e.Tax).HasColumnName("tax");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.PaymentGroup).HasColumnName("payment_group");
            entity.Property(e => e.AcumCompMin).HasColumnName("acum_comp_min");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Offer).HasColumnName("offer");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.HideCommissionable).HasColumnName("hide_commissionable");
            entity.Property(e => e.HidePoint).HasColumnName("hide_point");
            entity.Property(e => e.ActiveHtmlContent).HasColumnName("active_html_content");
            entity.Property(e => e.ActiveZoomPhotos).HasColumnName("active_zoom_photos");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Visible).HasColumnName("visible");
            entity.Property(e => e.VisiblePublic).HasColumnName("visible_public");
            entity.Property(e => e.ProductType).HasColumnName("product_type");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.ActivateCombinations).HasColumnName("activate_combinations");
            entity.Property(e => e.ProductPacks).HasColumnName("product_packs");
            entity.Property(e => e.BaseAmount).HasColumnName("base_amount");
            entity.Property(e => e.DailyPercentage).HasColumnName("daily_percentage");
            entity.Property(e => e.DaysWait).HasColumnName("days_wait");
            entity.Property(e => e.AmountDayPay).HasColumnName("amount_day_pay");
            entity.Property(e => e.RecurringProduct).HasColumnName("recurring_product");
            entity.Property(e => e.ProductHome).HasColumnName("product_home");
            entity.Property(e => e.AssociatedQualification).HasColumnName("associated_qualification");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionHtml).HasColumnName("description_html");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.Keyword).HasColumnName("keyword");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.ModelTwoPercentage).HasColumnName("model_two_percentage");
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_products_categories");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsCategory>(entity =>
        {
            entity.ToTable("products_categories", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.DisplaySmallBanner).HasColumnName("display_small_banner");
            entity.Property(e => e.DisplayBigBanner).HasColumnName("display_big_banner");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsInventory>(entity =>
        {
            entity.ToTable("products_inventories", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Ingress).HasColumnName("ingress");
            entity.Property(e => e.Egress).HasColumnName("egress");
            entity.Property(e => e.Support).HasColumnName("support");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdCombination).HasColumnName("id_combination");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasOne(d => d.IdProductNavigation)
                .WithMany(p => p.ProductsInventories)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_inventories_products");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsAttribute>(entity =>
        {
            entity.ToTable("products_attributes", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attribute).HasColumnName("attribute");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsAttributesValue>(entity =>
        {
            entity.ToTable("products_attributes_values", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAttribute).HasColumnName("id_attribute");
            entity.Property(e => e.AttributeValue).HasColumnName("attribute_value");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasOne(d => d.IdAttributeNavigation)
                .WithMany(p => p.ProductsAttributesValues)
                .HasForeignKey(d => d.IdAttribute)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_attributes_values_products_attributes");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsDiscount>(entity =>
        {
            entity.ToTable("products_discounts", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Qualification).HasColumnName("qualification");
            entity.Property(e => e.Percentage).HasColumnName("percentage");
            entity.Property(e => e.PointsQualify).HasColumnName("points_qualify");
            entity.Property(e => e.BinaryPoints).HasColumnName("binary_points");
            entity.Property(e => e.Commissionable).HasColumnName("commissionable");
            entity.Property(e => e.PCommissionable).HasColumnName("p_commissionable");
            entity.Property(e => e.PBinaryPoints).HasColumnName("p_binary_points");
            entity.Property(e => e.PPointsQualify).HasColumnName("p_points_qualify");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasOne(d => d.IdProductNavigation)
                .WithMany(p => p.ProductsDiscounts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_discounts_products");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsBanner>(entity =>
        {
            entity.ToTable("products_banners", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ViewInfo).HasColumnName("view_info");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ProductsCombination>(entity =>
        {
            entity.ToTable("products_combinations", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdAttributes).HasColumnName("id_attributes");
            entity.Property(e => e.CodeRef).HasColumnName("code_ref");
            entity.Property(e => e.DisplayBigBanner).HasColumnName("display_big_banner");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);
            entity.Property(e => e.BrandId).HasColumnName(ColBrandId);

            entity.HasOne(d => d.IdProductNavigation)
                .WithMany(p => p.ProductsCombinations)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_combinations_products");

            entity.HasOne(d => d.IdAttributesNavigation)
                .WithMany(p => p.ProductsCombinations)
                .HasForeignKey(d => d.IdAttributes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_combinations_products_attributes");

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("brands", SchemaName);
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SecretKey).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SecretKey).HasColumnName("secret_key");
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValueSql("true").HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });

        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.ToTable("api_clients", SchemaName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql(SqlCurrentTimestamp).HasColumnName(ColCreatedAt);
            entity.Property(e => e.UpdatedAt).HasColumnName(ColUpdatedAt);
            entity.Property(e => e.DeletedAt).HasColumnName(ColDeletedAt);

            entity.HasQueryFilter(e => !e.DeletedAt.HasValue);
        });
    }
}
