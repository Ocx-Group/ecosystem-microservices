using Ecosystem.WalletService.Application.Queries.UserStatistics;
using Ecosystem.WalletService.Domain.DTOs.AffiliateInformation;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.UserStatistics;

public class GetUserStatisticsHandler : IRequestHandler<GetUserStatisticsQuery, Domain.DTOs.AffiliateInformation.UserStatistics>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IResultsEcoPoolRepository _resultsEcoPoolRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetUserStatisticsHandler> _logger;

    public GetUserStatisticsHandler(
        IInvoiceRepository invoiceRepository,
        IResultsEcoPoolRepository resultsEcoPoolRepository,
        ITenantContext tenantContext,
        ILogger<GetUserStatisticsHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _resultsEcoPoolRepository = resultsEcoPoolRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<Domain.DTOs.AffiliateInformation.UserStatistics> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var brandId = _tenantContext.TenantId;

        var pointsModel4 = await _invoiceRepository.Model4StatisticsByUser(userId);
        var amountModel1A = await _invoiceRepository.CountDetailsByPaymentGroup(7, userId, brandId);
        var amountModel1B = await _invoiceRepository.CountDetailsByPaymentGroup(8, userId, brandId);
        var amountModel2 = await _invoiceRepository.CountDetailsByPaymentGroup(2, userId, brandId);
        var amountModel3 = await _invoiceRepository.CountDetailsModel3ByPaymentGroup(userId, brandId);

        var residualModel1A = await _resultsEcoPoolRepository.SumResidualModel1AByUserId(userId);
        var passiveModel1A = await _resultsEcoPoolRepository.SumPassiveModel1AByUserId(userId);
        var residualModel1B = await _resultsEcoPoolRepository.SumResidualModel1BByUserId(userId);
        var passiveModel1B = await _resultsEcoPoolRepository.SumPassiveModel1BByUserId(userId);
        var residualModel2 = await _resultsEcoPoolRepository.SumResidualModel2ByUserId(userId);
        var passiveModel2 = await _resultsEcoPoolRepository.SumPassiveModel2ByUserId(userId);
        var residualModel3 = await _resultsEcoPoolRepository.SumResidualModel3ByUserId(userId);
        var percentageModel3 = await _resultsEcoPoolRepository.SumPercentageModel3ByUserId(userId);

        return new Domain.DTOs.AffiliateInformation.UserStatistics
        {
            AmountPoolModel1A = amountModel1A,
            AmountPoolModel1B = amountModel1B,
            AmountPoolModel2 = amountModel2,
            AmountPoolModel3 = amountModel3,
            VolumeLeftModel4 = pointsModel4.Sum(s => s.CreditLeft - s.DebitLeft),
            VolumeRightModel4 = pointsModel4.Sum(s => s.CreditRight - s.DebitRight),
            AmountResidualModel1A = residualModel1A,
            AmountPassiveModel1A = passiveModel1A,
            AmountResidualModel1B = residualModel1B,
            AmountPassiveModel1B = passiveModel1B,
            AmountResidualModel2 = residualModel2,
            AmountPassiveModel2 = passiveModel2,
            AmountResidualModel3 = residualModel3,
            AmountPercentageModel3 = percentageModel3
        };
    }
}
