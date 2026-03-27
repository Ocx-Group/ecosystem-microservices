using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Configuration;


namespace Ecosystem.WalletService.Data.Repositories;

public class ResultsEcoPoolRepository : BaseRepository, IResultsEcoPoolRepository
{
    private readonly ApplicationConfiguration _appSettings;
    public ResultsEcoPoolRepository(IOptions<ApplicationConfiguration> appSettings, WalletServiceDbContext context) : base(context)
        => _appSettings = appSettings.Value;

    public Task<List<ResultsModel2>> GetAllResultsEcoPool()
        => Context.ResultsModel2
            .Include(x => x.ResultsModel2Levels).AsNoTracking()
            .Where(x => x.CompletedAt == null).ToListAsync();

    public Task<List<ResultsModel2>> GetResultsModel2ToPayment()
    {
        return Context.ResultsModel2
            .Include(x => x.ModelConfiguration)
            .Include(x => x.ResultsModel2Levels)
            .Where(x => x.ModelConfiguration.CompletedAt == null).ToListAsync();
    }

    public Task<List<ResultsModel1a>> GetResultsModel1AToPayment()
    {
        return Context.ResultsModel1A
            .Include(x => x.ResultsModel1aLevels).ToListAsync();
    }

    public Task<List<ResultsModel1b>> GetResultsModel1BToPayment()
    {
        return Context.ResultsModel1B
            .Include(x => x.ResultsModel1bLevels).ToListAsync();
    }

    public Task<List<ResultsModel3>> GetResultsMode3ToPayment()
    {
        return Context.ResultsModel3
            .Include(x => x.ResultsModel3Levels)
            .Where(x => x.CompletedAt == null).ToListAsync();
    }

    public Task<decimal> SumResidualModel1AByUserId(int userId)
    {
        return Context.ResultsModel1ALevels.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumPassiveModel1AByUserId(int userId)
    {
        return Context.ResultsModel1A.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }
    
    public Task<decimal> SumResidualModel1BByUserId(int userId)
    {
        return Context.ResultsModel1BLevels.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumPassiveModel1BByUserId(int userId)
    {
        return Context.ResultsModel1B.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumResidualModel2ByUserId(int userId)
    {
        return Context.ResultsModel2Levels.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumPassiveModel2ByUserId(int userId)
    {
        return Context.ResultsModel2.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumResidualModel3ByUserId(int userId)
    {
        return Context.ResultsModel3Levels.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);
    }

    public Task<decimal> SumPercentageModel3ByUserId(int userId)
    {
        return Context.ResultsModel3.Where(x => x.AffiliateId == userId)
            .SumAsync(s => s.PaymentAmount);    
    }
}