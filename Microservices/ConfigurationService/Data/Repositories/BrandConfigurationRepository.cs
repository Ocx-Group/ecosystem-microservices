using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class BrandConfigurationRepository : BaseRepository, IBrandConfigurationRepository
{
    public BrandConfigurationRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<BrandConfiguration?> GetByBrandIdAsync(long brandId)
        => Context.BrandConfigurations
            .Include(x => x.Brand)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BrandId == brandId);

    public Task<List<BrandConfiguration>> GetAllAsync()
        => Context.BrandConfigurations
            .Include(x => x.Brand)
            .AsNoTracking()
            .ToListAsync();

    public async Task<BrandConfiguration> UpsertAsync(BrandConfiguration config)
    {
        var existing = await Context.BrandConfigurations
            .FirstOrDefaultAsync(x => x.BrandId == config.BrandId);

        var now = DateTime.UtcNow;

        if (existing is null)
        {
            config.CreatedAt = now;
            config.UpdatedAt = now;
            await Context.BrandConfigurations.AddAsync(config);
        }
        else
        {
            existing.AdminUserName = config.AdminUserName;
            existing.SenderName = config.SenderName;
            existing.SenderEmail = config.SenderEmail;
            existing.EmailTemplateFolder = config.EmailTemplateFolder;
            existing.ClientUrl = config.ClientUrl;
            existing.CommissionEnabled = config.CommissionEnabled;
            existing.CommissionLevelsJson = config.CommissionLevelsJson;
            existing.BonusPercentage = config.BonusPercentage;
            existing.PdfTemplateName = config.PdfTemplateName;
            existing.CompanyName = config.CompanyName;
            existing.CompanyIdentifier = config.CompanyIdentifier;
            existing.SupportEmail = config.SupportEmail;
            existing.SupportPhone = config.SupportPhone;
            existing.DocumentType = config.DocumentType;
            existing.LogoUrl = config.LogoUrl;
            existing.PrimaryColor = config.PrimaryColor;
            existing.SecondaryColor = config.SecondaryColor;
            existing.BackgroundColor = config.BackgroundColor;
            existing.DefaultFatherAffiliateId = config.DefaultFatherAffiliateId;
            existing.ActivateOnRegistration = config.ActivateOnRegistration;
            existing.DefaultPaymentGroupId = config.DefaultPaymentGroupId;
            existing.TradingAcademyPaymentGroupId = config.TradingAcademyPaymentGroupId;
            existing.WithdrawalValidationType = config.WithdrawalValidationType;
            existing.WithdrawalTimeZone = config.WithdrawalTimeZone;
            existing.WithdrawalStartHour = config.WithdrawalStartHour;
            existing.WithdrawalEndHour = config.WithdrawalEndHour;
            existing.WithdrawalCapNoDirects = config.WithdrawalCapNoDirects;
            existing.Requires10PercentPurchaseRule = config.Requires10PercentPurchaseRule;
            existing.PoolValidationRequired = config.PoolValidationRequired;
            existing.ConPaymentEnabled = config.ConPaymentEnabled;
            existing.ConPaymentAddress = config.ConPaymentAddress;
            existing.BlockchainNetworkId = config.BlockchainNetworkId;
            existing.IsActive = config.IsActive;
            existing.UpdatedAt = now;

            config = existing;
        }

        await Context.SaveChangesAsync();
        return config;
    }

    public async Task<BrandConfiguration?> DeleteAsync(long brandId)
    {
        var existing = await Context.BrandConfigurations
            .FirstOrDefaultAsync(x => x.BrandId == brandId);

        if (existing is null) return null;

        existing.DeletedAt = DateTime.UtcNow;
        existing.UpdatedAt = DateTime.UtcNow;
        Context.BrandConfigurations.Update(existing);
        await Context.SaveChangesAsync();

        return existing;
    }
}
