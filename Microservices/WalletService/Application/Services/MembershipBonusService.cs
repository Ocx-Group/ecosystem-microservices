using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Services;

public class MembershipBonusService : IMembershipBonusService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly ILogger<MembershipBonusService> _logger;

    public MembershipBonusService(
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountAdapter,
        ILogger<MembershipBonusService> logger)
    {
        _walletRepository = walletRepository;
        _accountAdapter = accountAdapter;
        _logger = logger;
    }

    public async Task CreditBonusToParentAsync(UserInfoResponse userInfo, WalletRequestModel request)
    {
        if (userInfo.Father <= 0) return;

        var parentInfo = await _accountAdapter.GetUserInfo(userInfo.Father, request.BrandId);
        if (parentInfo == null) return;

        try
        {
            var creditRequest = new CreditTransactionRequest
            {
                AffiliateId = parentInfo.Id,
                UserId = Constants.AdminUserId,
                Concept = $"{Constants.CommissionMembership} {request.AffiliateUserName}",
                Credit = Constants.MembershipBonus,
                AffiliateUserName = parentInfo.UserName,
                ConceptType = nameof(WalletConceptType.membership_bonus),
                BrandId = request.BrandId
            };

            await _walletRepository.CreditTransaction(creditRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error crediting membership bonus to parent {ParentId} for affiliate {AffiliateId}",
                userInfo.Father, request.AffiliateId);
        }
    }
}
