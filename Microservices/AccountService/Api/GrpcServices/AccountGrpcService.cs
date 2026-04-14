using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Affiliate;
using Ecosystem.AccountService.Application.Commands.Matrix;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Application.Queries.AffiliateBtc;
using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.Grpc.Account;
using Grpc.Core;
using MediatR;

namespace Ecosystem.AccountService.Api.GrpcServices;

public class AccountGrpcService : AccountGrpc.AccountGrpcBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountGrpcService> _logger;

    public AccountGrpcService(IMediator mediator, IMapper mapper, ILogger<AccountGrpcService> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    private static void SetTenant(ServerCallContext context, long brandId)
    {
        var httpContext = context.GetHttpContext();
        httpContext.Items["tenantId"] = brandId;
        httpContext.Items["brandId"] = brandId;
    }

    public override async Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var affiliate = await _mediator.Send(new GetAffiliateByIdQuery(request.UserId));
            if (affiliate is null)
                return new GetUserInfoResponse();

            return new GetUserInfoResponse { User = _mapper.Map<UserInfoMessage>(affiliate) };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUserInfo for user {UserId}", request.UserId);
            return new GetUserInfoResponse();
        }
    }

    public override async Task<GetAffiliateByUserNameResponse> GetAffiliateByUserName(
        GetAffiliateByUserNameRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var affiliate = await _mediator.Send(new GetAffiliateByUserNameQuery(request.UserName));
            if (affiliate is null)
                return new GetAffiliateByUserNameResponse { Success = false, Message = "User not found" };

            return new GetAffiliateByUserNameResponse
            {
                Success = true,
                User = _mapper.Map<UserInfoMessage>(affiliate)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAffiliateByUserName for {UserName}", request.UserName);
            return new GetAffiliateByUserNameResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<PlaceUserInMatrixResponse> PlaceUserInMatrix(
        PlaceUserInMatrixRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var result = await _mediator.Send(new PlaceUserInMatrixCommand(
                request.UserId,
                request.MatrixType,
                request.RecipientId == 0 ? null : request.RecipientId,
                request.Cycle == 0 ? null : request.Cycle));

            return new PlaceUserInMatrixResponse
            {
                Success = result,
                Message = result ? "User placed in matrix" : "Failed to place user in matrix"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PlaceUserInMatrix for user {UserId}", request.UserId);
            return new PlaceUserInMatrixResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<IsActiveInMatrixResponse> IsActiveInMatrix(
        IsActiveInMatrixRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var isActive = await _mediator.Send(new IsActiveInMatrixQuery(
                request.UserId, request.MatrixType, request.Cycle));

            return new IsActiveInMatrixResponse { Success = true, IsActive = isActive };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in IsActiveInMatrix for user {UserId}", request.UserId);
            return new IsActiveInMatrixResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetUplinePositionsResponse> GetUplinePositions(
        GetUplinePositionsRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var positions = await _mediator.Send(new GetUplinePositionsQuery(
                request.UserId, request.MatrixType, request.Cycle));

            var response = new GetUplinePositionsResponse { Success = true };

            if (positions is not null)
            {
                foreach (var pos in positions)
                    response.Positions.Add(_mapper.Map<MatrixPositionMessage>(pos));
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetUplinePositions for user {UserId}", request.UserId);
            return new GetUplinePositionsResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<VerificationCodeResponse> VerificationCode(
        VerificationCodeRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var result = await _mediator.Send(new ValidationCodeCommand(
                request.AffiliateId, request.Code, request.Password));

            return new VerificationCodeResponse
            {
                Success = result,
                Message = result ? "Verification successful" : "Verification failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VerificationCode for affiliate {AffiliateId}", request.AffiliateId);
            return new VerificationCodeResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetAffiliateBtcByAffiliateIdResponse> GetAffiliateBtcByAffiliateId(
        GetAffiliateBtcByAffiliateIdRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var addresses = await _mediator.Send(new GetAffiliateBtcByAffiliateIdQuery(request.AffiliateId));

            var response = new GetAffiliateBtcByAffiliateIdResponse { Success = true };

            if (addresses is not null)
            {
                foreach (var addr in addresses)
                    response.Addresses.Add(_mapper.Map<AffiliateBtcMessage>(addr));
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAffiliateBtcByAffiliateId for {AffiliateId}", request.AffiliateId);
            return new GetAffiliateBtcByAffiliateIdResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetPersonalNetworkResponse> GetPersonalNetwork(
        GetPersonalNetworkRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var members = await _mediator.Send(new GetPersonalNetworkQuery(request.AffiliateId));

            var response = new GetPersonalNetworkResponse { Success = true };

            foreach (var member in members)
                response.Members.Add(_mapper.Map<PersonalNetworkMessage>(member));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPersonalNetwork for affiliate {AffiliateId}", request.AffiliateId);
            return new GetPersonalNetworkResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetTotalActiveMembersResponse> GetTotalActiveMembers(
        GetTotalActiveMembersRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var total = await _mediator.Send(new GetTotalActiveMembersQuery());

            return new GetTotalActiveMembersResponse { Success = true, Total = total };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTotalActiveMembers");
            return new GetTotalActiveMembersResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetHave2ChildrenResponse> GetHave2Children(
        GetHave2ChildrenRequest request, ServerCallContext context)
    {
        try
        {
            SetTenant(context, request.BrandId);
            var userIds = request.AffiliateIds.Select(id => (long)id).ToArray();
            var result = await _mediator.Send(new WhatUsersHave2ChildrenQuery(userIds));

            var response = new GetHave2ChildrenResponse { Success = true };
            response.AffiliateIdsWithChildren.AddRange(result.Select(id => (int)id));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetHave2Children");
            return new GetHave2ChildrenResponse { Success = false, Message = "Internal error" };
        }
    }
}