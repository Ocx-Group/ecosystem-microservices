using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Matrix;

public class WhatUsersHave2ChildrenHandler : IRequestHandler<WhatUsersHave2ChildrenQuery, long[]>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ILogger<WhatUsersHave2ChildrenHandler> _logger;

    public WhatUsersHave2ChildrenHandler(
        IUserAffiliateInfoRepository repo,
        ILogger<WhatUsersHave2ChildrenHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<long[]> Handle(WhatUsersHave2ChildrenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Checking which users have 2 children for {Count} users", request.Users.Length);
        return await _repo.WhatUsersHave2Children(request.Users);
    }
}
