using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Constants;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<Product>> GetAllEcoPools()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.EcoPools)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllProductsByIds(long[] ids)
        => Context.Products
            .Where(x => ids.Contains(x.Id))
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAlMembership(long brandId)
        => Context.Products
            .Where(x => x.ProductType == InventoryConstants.Membership && x.BrandId == brandId)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<Product?> GetProductsById(int id)
        => Context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Product> CreateProductAsync(Product request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.Products.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<Product> UpdateProductAsync(Product request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.Products.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<Product> DeleteProduct(Product request)
    {
        request.DeletedAt = DateTime.Now;
        Context.Products.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public Task<List<Product>> GetAllProductsAdmin(long brandId)
        => Context.Products
            .Where(x => x.ProductType == InventoryConstants.Products && x.BrandId == brandId)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllRecurring()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.Recurring)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllServices()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.Services)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllFundingAccounts()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.FundingAccounts)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllTradingAcademy(int paymentGroup)
        => Context.Products
            .Where(x => x.PaymentGroup == paymentGroup)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllSavingsPlans()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.SavingsPlans)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllSavingsPlansOneB()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.SavingsPlansOneB)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllAlternativeHealth()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.AlternativeHealth)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllAlternativeHealthForEurope()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.AlternativeHealthForEurope)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllRecyCoin()
        => Context.Products
            .Where(x => x.PaymentGroup == InventoryConstants.RecyCoin)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<Product>> GetAllWithPaymentGroup(int paymentGroup, long brandId)
        => Context.Products
            .Where(x => x.PaymentGroup == paymentGroup && x.BrandId == brandId)
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
}
