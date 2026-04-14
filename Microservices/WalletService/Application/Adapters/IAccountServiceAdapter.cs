using Ecosystem.WalletService.Domain.DTOs.AffiliateBtc;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IAccountServiceAdapter
{
    Task<UserInfoResponse?> GetUserInfo(int userId, long brandId);
    Task<UserInfoResponse?> GetAffiliateByUserName(string userName, long brandId);
    Task<bool> PlaceUserInMatrix(MatrixRequest request, long brandId);
    Task<bool> IsActiveInMatrix(MatrixRequest request, long brandId);
    Task<List<MatrixPositionDto>?> GetUplinePositionsAsync(MatrixRequest request, long brandId);
    Task<bool> VerificationCode(string code, string password, int affiliateId, long brandId);
    Task<List<AffiliateBtcDto>?> GetAffiliateBtcByAffiliateId(int affiliateId, long brandId);
    Task<List<PersonalNetwork>?> GetPersonalNetwork(int affiliateId, long brandId);
    Task<int> GetTotalActiveMembers(long brandId);
    Task<int[]?> GetHave2Children(int[] affiliateIds, long brandId);
}
