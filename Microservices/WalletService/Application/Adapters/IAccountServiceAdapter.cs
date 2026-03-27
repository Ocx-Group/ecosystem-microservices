using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Replace HTTP calls with gRPC/RabbitMQ inter-service communication
public interface IAccountServiceAdapter
{
    Task<UserInfoResponse?> GetUserInfo(int userId, long brandId);
    Task<IRestResponse> PlaceUserInMatrix(MatrixRequest request, long brandId);
    Task<IRestResponse> IsActiveInMatrix(MatrixRequest request, long brandId);
    Task<IRestResponse> GetUplinePositionsAsync(MatrixRequest request, long brandId);
    Task<RestResponse> GetAffiliateByUserName(string userName, long brandId);
    Task<RestResponse> VerificationCode(string code, string password, int affiliateId, long brandId);
    Task<RestResponse> GetAffiliateBtcByAffiliateId(int affiliateId, long brandId);
    Task<RestResponse> GetPersonalNetwork(int affiliateId, long brandId);
    Task<RestResponse> GetTotalActiveMembers(long brandId);
    Task<RestResponse> GetHave2Children(int[] affiliateIds, long brandId);
}
