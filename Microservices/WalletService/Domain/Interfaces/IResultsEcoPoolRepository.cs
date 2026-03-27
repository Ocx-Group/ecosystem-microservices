using Ecosystem.WalletService.Domain.Models;


namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IResultsEcoPoolRepository
{
    Task<List<ResultsModel2>> GetAllResultsEcoPool();
    Task<List<ResultsModel2>> GetResultsModel2ToPayment();
    Task<List<ResultsModel1a>> GetResultsModel1AToPayment();
    Task<List<ResultsModel1b>> GetResultsModel1BToPayment();
    Task<List<ResultsModel3>> GetResultsMode3ToPayment();
    Task<decimal> SumResidualModel1AByUserId(int userId);
    Task<decimal> SumPassiveModel1AByUserId(int userId);
    Task<decimal> SumResidualModel1BByUserId(int userId);
    Task<decimal> SumPassiveModel1BByUserId(int userId);
    Task<decimal> SumResidualModel2ByUserId(int userId);
    Task<decimal> SumPassiveModel2ByUserId(int userId);
    Task<decimal> SumResidualModel3ByUserId(int userId);
    Task<decimal> SumPercentageModel3ByUserId(int userId);
}