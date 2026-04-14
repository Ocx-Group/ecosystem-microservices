using AutoMapper;
using Ecosystem.Grpc.Account;
using Ecosystem.WalletService.Domain.DTOs.AffiliateBtc;
using Ecosystem.WalletService.Domain.Requests.MatrixRequest;
using Ecosystem.WalletService.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Adapters;

public class GrpcAccountServiceAdapter : IAccountServiceAdapter
{
    private readonly AccountGrpc.AccountGrpcClient _client;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcAccountServiceAdapter> _logger;

    public GrpcAccountServiceAdapter(
        AccountGrpc.AccountGrpcClient client,
        IMapper mapper,
        ILogger<GrpcAccountServiceAdapter> logger)
    {
        _client = client;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserInfoResponse?> GetUserInfo(int userId, long brandId)
    {
        try
        {
            var response = await _client.GetUserInfoAsync(new GetUserInfoRequest
            {
                UserId = userId,
                BrandId = brandId
            });
            return response.User is null ? null : _mapper.Map<UserInfoResponse>(response.User);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetUserInfo for user {UserId}", userId);
            return null;
        }
    }

    public async Task<UserInfoResponse?> GetAffiliateByUserName(string userName, long brandId)
    {
        try
        {
            var response = await _client.GetAffiliateByUserNameAsync(new GetAffiliateByUserNameRequest
            {
                UserName = userName,
                BrandId = brandId
            });
            if (!response.Success || response.User is null) return null;
            return _mapper.Map<UserInfoResponse>(response.User);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetAffiliateByUserName for {UserName}", userName);
            return null;
        }
    }

    public async Task<bool> PlaceUserInMatrix(MatrixRequest request, long brandId)
    {
        try
        {
            var response = await _client.PlaceUserInMatrixAsync(new PlaceUserInMatrixRequest
            {
                UserId = request.UserId,
                MatrixType = request.MatrixType,
                RecipientId = request.RecipientId ?? 0,
                Cycle = request.Cycle ?? 0,
                BrandId = brandId
            });
            return response.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in PlaceUserInMatrix for user {UserId}", request.UserId);
            return false;
        }
    }

    public async Task<bool> IsActiveInMatrix(MatrixRequest request, long brandId)
    {
        try
        {
            var response = await _client.IsActiveInMatrixAsync(new IsActiveInMatrixRequest
            {
                UserId = request.UserId,
                MatrixType = request.MatrixType,
                Cycle = request.Cycle ?? 0,
                BrandId = brandId
            });
            return response.IsActive;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in IsActiveInMatrix for user {UserId}", request.UserId);
            return false;
        }
    }

    public async Task<List<MatrixPositionDto>?> GetUplinePositionsAsync(MatrixRequest request, long brandId)
    {
        try
        {
            var response = await _client.GetUplinePositionsAsync(new GetUplinePositionsRequest
            {
                UserId = request.UserId,
                MatrixType = request.MatrixType,
                Cycle = request.Cycle ?? 0,
                BrandId = brandId
            });
            if (!response.Success) return null;
            return response.Positions.Select(p => _mapper.Map<MatrixPositionDto>(p)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetUplinePositions for user {UserId}", request.UserId);
            return null;
        }
    }

    public async Task<bool> VerificationCode(string code, string password, int affiliateId, long brandId)
    {
        try
        {
            var response = await _client.VerificationCodeAsync(new VerificationCodeRequest
            {
                Code = code,
                Password = password,
                AffiliateId = affiliateId,
                BrandId = brandId
            });
            return response.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in VerificationCode for affiliate {AffiliateId}", affiliateId);
            return false;
        }
    }

    public async Task<List<AffiliateBtcDto>?> GetAffiliateBtcByAffiliateId(int affiliateId, long brandId)
    {
        try
        {
            var response = await _client.GetAffiliateBtcByAffiliateIdAsync(new GetAffiliateBtcByAffiliateIdRequest
            {
                AffiliateId = affiliateId,
                BrandId = brandId
            });
            if (!response.Success) return null;
            return response.Addresses.Select(a => _mapper.Map<AffiliateBtcDto>(a)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetAffiliateBtcByAffiliateId for {AffiliateId}", affiliateId);
            return null;
        }
    }

    public async Task<List<PersonalNetwork>?> GetPersonalNetwork(int affiliateId, long brandId)
    {
        try
        {
            var response = await _client.GetPersonalNetworkAsync(new GetPersonalNetworkRequest
            {
                AffiliateId = affiliateId,
                BrandId = brandId
            });
            if (!response.Success) return null;
            return response.Members.Select(m => _mapper.Map<PersonalNetwork>(m)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetPersonalNetwork for {AffiliateId}", affiliateId);
            return null;
        }
    }

    public async Task<int> GetTotalActiveMembers(long brandId)
    {
        try
        {
            var response = await _client.GetTotalActiveMembersAsync(new GetTotalActiveMembersRequest
            {
                BrandId = brandId
            });
            return response.Success ? response.Total : 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetTotalActiveMembers");
            return 0;
        }
    }

    public async Task<int[]?> GetHave2Children(int[] affiliateIds, long brandId)
    {
        try
        {
            var request = new GetHave2ChildrenRequest { BrandId = brandId };
            request.AffiliateIds.AddRange(affiliateIds);
            var response = await _client.GetHave2ChildrenAsync(request);
            if (!response.Success) return null;
            return response.AffiliateIdsWithChildren.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetHave2Children");
            return null;
        }
    }
}
