using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Matrix;

public class IsActiveInMatrixHandler : IRequestHandler<IsActiveInMatrixQuery, bool>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ILogger<IsActiveInMatrixHandler> _logger;

    public IsActiveInMatrixHandler(
        IUserAffiliateInfoRepository repo,
        ILogger<IsActiveInMatrixHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<bool> Handle(IsActiveInMatrixQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0)
            return false;

        return await _repo.IsUserActiveInMatrixAsync(request.UserId, request.MatrixType, request.Cycle);
    }
}
