using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllEcoPools();
    Task<List<Product>> GetAllProductsByIds(long[] ids);
    Task<List<Product>> GetAlMembership(long brandId);
    Task<Product?> GetProductsById(int id);
    Task<Product> CreateProductAsync(Product request);
    Task<Product> UpdateProductAsync(Product request);
    Task<Product> DeleteProduct(Product request);
    Task<List<Product>> GetAllProductsAdmin(long brandId);
    Task<List<Product>> GetAllRecurring();
    Task<List<Product>> GetAllServices();
    Task<List<Product>> GetAllFundingAccounts();
    Task<List<Product>> GetAllTradingAcademy(int paymentGroup);
    Task<List<Product>> GetAllSavingsPlans();
    Task<List<Product>> GetAllSavingsPlansOneB();
    Task<List<Product>> GetAllAlternativeHealth();
    Task<List<Product>> GetAllAlternativeHealthForEurope();
    Task<List<Product>> GetAllRecyCoin();
    Task<List<Product>> GetAllWithPaymentGroup(int paymentGroup, long brandId);
}
