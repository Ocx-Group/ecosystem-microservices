using Ecosystem.Grpc.Wallet;
using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using Grpc.Core;
using MediatR;

namespace Ecosystem.WalletService.Api.GrpcServices;

public class WalletGrpcService : WalletGrpc.WalletGrpcBase
{
    private readonly IMediator _mediator;

    public WalletGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<IsWithdrawalDateAllowedResponse> IsWithdrawalDateAllowed(
        IsWithdrawalDateAllowedRequest request, ServerCallContext context)
    {
        var allowed = await _mediator.Send(new IsWithdrawalDateAllowedQuery());

        return new IsWithdrawalDateAllowedResponse { Allowed = allowed };
    }
}
