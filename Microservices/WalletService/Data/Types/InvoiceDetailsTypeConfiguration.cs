using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Data.Types;

public class InvoiceDetailsTypeConfiguration : IEntityTypeConfiguration<InvoiceDetailsTransactionRequest>
{
    public void Configure(EntityTypeBuilder<InvoiceDetailsTransactionRequest> builder)
    {
        builder.HasNoKey();
        builder.ToTable("invoices_details_type_with_brand", "wallet_service");

        builder.Property(x => x.ProductId).HasColumnType("integer");
        builder.Property(x => x.PaymentGroupId).HasColumnType("integer");
        builder.Property(x => x.AccumMinPurchase).HasColumnType("boolean");
        builder.Property(x => x.ProductName).HasColumnType("varchar");
        builder.Property(x => x.ProductPrice).HasColumnType("numeric");
        builder.Property(x => x.ProductPriceBtc).HasColumnType("numeric");
        builder.Property(x => x.ProductIva).HasColumnType("numeric");
        builder.Property(x => x.ProductQuantity).HasColumnType("integer");
        builder.Property(x => x.ProductCommissionable).HasColumnType("numeric");
        builder.Property(x => x.BinaryPoints).HasColumnType("numeric");
        builder.Property(x => x.ProductPoints).HasColumnType("numeric");
        builder.Property(x => x.ProductDiscount).HasColumnType("numeric");
        builder.Property(x => x.CombinationId).HasColumnType("integer");
        builder.Property(x => x.ProductPack).HasColumnType("boolean");
        builder.Property(x => x.BaseAmount).HasColumnType("numeric");
        builder.Property(x => x.DailyPercentage).HasColumnType("numeric");
        builder.Property(x => x.WaitingDays).HasColumnType("integer");
        builder.Property(x => x.DaysToPayQuantity).HasColumnType("integer");
        builder.Property(x => x.ProductStart).HasColumnType("boolean");
        builder.Property(x => x.BrandId).HasColumnType("integer");
    }
}