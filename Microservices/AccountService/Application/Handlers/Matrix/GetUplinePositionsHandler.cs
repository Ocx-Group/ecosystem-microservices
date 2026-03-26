using Ecosystem.AccountService.Application.Adapters;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Matrix;

public class GetUplinePositionsHandler : IRequestHandler<GetUplinePositionsQuery, IEnumerable<MatrixPositionDto>?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly IConfigurationServiceAdapter _configurationServiceAdapter;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetUplinePositionsHandler> _logger;

    public GetUplinePositionsHandler(
        IUserAffiliateInfoRepository repo,
        IConfigurationServiceAdapter configurationServiceAdapter,
        ITenantContext tenantContext,
        ILogger<GetUplinePositionsHandler> logger)
    {
        _repo = repo;
        _configurationServiceAdapter = configurationServiceAdapter;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<MatrixPositionDto>?> Handle(GetUplinePositionsQuery request, CancellationToken cancellationToken)
    {
        const int targetLeaders = 7;
        var leaders = new List<MatrixPositionDto>(targetLeaders);
        var processed = new HashSet<int> { request.UserId };

        // 0. Should we prioritize children?
        bool searchChildrenFirst = false;
        int? fatherId = null;

        var myInfo = await _repo.GetAffiliateByIdAsync(request.UserId, _tenantContext.TenantId);

        if (myInfo?.Father is { } fId and > 0)
        {
            fatherId = fId;
            int activeDirects = await CountActiveChildrenInMatrixAsync(fatherId.Value, request.MatrixType);
            searchChildrenFirst = activeDirects >= 3;
        }

        // 1. Build the parent chain (without adding them yet)
        var parentCandidates = new List<int>();
        int? currentId = request.UserId;
        while (currentId is not null)
        {
            var info = await _repo.GetAffiliateByIdAsync(currentId.Value, _tenantContext.TenantId);

            if (info?.Father is null or 0) break;
            currentId = info.Father;

            bool isActive = await _repo.IsUserActiveInMatrixAsync(currentId.Value, request.MatrixType, request.Cycle);

            if (isActive)
                parentCandidates.Add(currentId.Value);
        }

        // 1.5 Pay father first (when children are prioritized)
        if (searchChildrenFirst && fatherId is { } fid)
        {
            await AddLeaderIfInMatrixAsync(fid, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
            if (leaders.Count >= targetLeaders) return leaders;
        }

        // 2. Parents first (only if NOT prioritizing children)
        if (!searchChildrenFirst)
        {
            foreach (var pid in parentCandidates)
            {
                await AddLeaderIfInMatrixAsync(pid, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                if (leaders.Count >= targetLeaders) return leaders;
            }
        }

        // 3. Children
        foreach (var child in await GetChildrenIdsAsync(request.UserId))
        {
            await AddLeaderIfInMatrixAsync(child, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
            if (leaders.Count >= targetLeaders) return leaders;
        }

        // 4. Siblings
        var meInfo = myInfo;
        if (meInfo?.Father is { } myFather and > 0)
        {
            var siblings = (await GetChildrenIdsAsync(myFather))
                .Where(s => s != request.UserId)
                .ToList();

            foreach (var sib in siblings)
            {
                await AddLeaderIfInMatrixAsync(sib, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                if (leaders.Count >= targetLeaders) break;
            }

            // 5. Children of siblings and descendants up to 6 levels deep
            foreach (var sib in siblings)
            {
                foreach (var nephew in await GetChildrenIdsAsync(sib))
                {
                    await AddLeaderIfInMatrixAsync(nephew, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                    if (leaders.Count >= targetLeaders) break;

                    foreach (var nephewChild in await GetChildrenIdsAsync(nephew))
                    {
                        await AddLeaderIfInMatrixAsync(nephewChild, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                        if (leaders.Count >= targetLeaders) break;

                        foreach (var nephewGrand in await GetChildrenIdsAsync(nephewChild))
                        {
                            await AddLeaderIfInMatrixAsync(nephewGrand, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                            if (leaders.Count >= targetLeaders) break;

                            foreach (var nephewGreat in await GetChildrenIdsAsync(nephewGrand))
                            {
                                await AddLeaderIfInMatrixAsync(nephewGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                if (leaders.Count >= targetLeaders) break;

                                foreach (var nephew2XGreat in await GetChildrenIdsAsync(nephewGreat))
                                {
                                    await AddLeaderIfInMatrixAsync(nephew2XGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                    if (leaders.Count >= targetLeaders) break;

                                    foreach (var nephew3XGreat in await GetChildrenIdsAsync(nephew2XGreat))
                                    {
                                        await AddLeaderIfInMatrixAsync(nephew3XGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                        if (leaders.Count >= targetLeaders) break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 6. Uncles, cousins, and extended ancestry
            var fatherInfo = await _repo.GetAffiliateByIdAsync(myFather, _tenantContext.TenantId);
            if (fatherInfo?.Father is { } grandFather and > 0)
            {
                var uncles = (await GetChildrenIdsAsync(grandFather))
                    .Where(u => u != myFather)
                    .ToList();

                foreach (var uncle in uncles)
                {
                    await AddLeaderIfInMatrixAsync(uncle, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                    if (leaders.Count >= targetLeaders) break;
                }

                foreach (var uncle in uncles)
                {
                    foreach (var cousin in await GetChildrenIdsAsync(uncle))
                    {
                        await AddLeaderIfInMatrixAsync(cousin, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                        if (leaders.Count >= targetLeaders) break;

                        foreach (var cousinChild in await GetChildrenIdsAsync(cousin))
                        {
                            await AddLeaderIfInMatrixAsync(cousinChild, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                            if (leaders.Count >= targetLeaders) break;

                            foreach (var cousinGrandChild in await GetChildrenIdsAsync(cousinChild))
                            {
                                await AddLeaderIfInMatrixAsync(cousinGrandChild, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                if (leaders.Count >= targetLeaders) break;

                                foreach (var cousinGreatGrand in await GetChildrenIdsAsync(cousinGrandChild))
                                {
                                    await AddLeaderIfInMatrixAsync(cousinGreatGrand, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                    if (leaders.Count >= targetLeaders) break;

                                    foreach (var cousin2XGreat in await GetChildrenIdsAsync(cousinGreatGrand))
                                    {
                                        await AddLeaderIfInMatrixAsync(cousin2XGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                        if (leaders.Count >= targetLeaders) break;

                                        foreach (var cousin3XGreat in await GetChildrenIdsAsync(cousin2XGreat))
                                        {
                                            await AddLeaderIfInMatrixAsync(cousin3XGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                            if (leaders.Count >= targetLeaders) break;

                                            foreach (var cousin4XGreat in await GetChildrenIdsAsync(cousin3XGreat))
                                            {
                                                await AddLeaderIfInMatrixAsync(cousin4XGreat, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                                                if (leaders.Count >= targetLeaders) break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // 7. Ancestry (grandfather, great-grandfather, etc.)
                await AddLeaderIfInMatrixAsync(grandFather, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                if (leaders.Count >= targetLeaders) return leaders;

                var grandFatherInfo = await _repo.GetAffiliateByIdAsync(grandFather, _tenantContext.TenantId);
                if (grandFatherInfo?.Father is { } greatGrand and > 0)
                {
                    await AddLeaderIfInMatrixAsync(greatGrand, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                    if (leaders.Count >= targetLeaders) return leaders;

                    var greatUncles = (await GetChildrenIdsAsync(greatGrand))
                        .Where(id => id != grandFather)
                        .ToList();

                    foreach (var gUncle in greatUncles)
                    {
                        await AddLeaderIfInMatrixAsync(gUncle, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                        if (leaders.Count >= targetLeaders) break;
                    }

                    foreach (var gUncle in greatUncles)
                    {
                        var queue = new Queue<int>(await GetChildrenIdsAsync(gUncle));
                        while (queue.Count > 0 && leaders.Count < targetLeaders)
                        {
                            var descendantId = queue.Dequeue();
                            await AddLeaderIfInMatrixAsync(descendantId, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                            if (leaders.Count >= targetLeaders) break;

                            foreach (var child in await GetChildrenIdsAsync(descendantId))
                                queue.Enqueue(child);
                        }
                    }
                }
            }

            // 8+. Ascend indefinitely
            var ancestor = currentId;
            while (leaders.Count < targetLeaders && ancestor is not null)
            {
                var ancestorInfo = await _repo.GetAffiliateByIdAsync(ancestor.Value, _tenantContext.TenantId);

                var nextAncestor = ancestorInfo?.Father ?? 0;
                if (nextAncestor <= 0) break;
                ancestor = nextAncestor;

                await AddLeaderIfInMatrixAsync(ancestor.Value, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                if (leaders.Count >= targetLeaders) break;

                var ancestorSiblings = (await GetChildrenIdsAsync(ancestor.Value))
                    .Where(id => id != ancestor.Value)
                    .ToList();

                foreach (var sib in ancestorSiblings)
                {
                    await AddLeaderIfInMatrixAsync(sib, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                    if (leaders.Count >= targetLeaders) break;
                }

                foreach (var sib in ancestorSiblings)
                {
                    foreach (var cousin in await GetChildrenIdsAsync(sib))
                    {
                        await AddLeaderIfInMatrixAsync(cousin, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                        if (leaders.Count >= targetLeaders) break;
                    }
                }
            }
        }

        // 9. Parents at end if they were deferred
        if (searchChildrenFirst && leaders.Count < targetLeaders)
        {
            foreach (var pid in parentCandidates)
            {
                await AddLeaderIfInMatrixAsync(pid, request.MatrixType, request.Cycle, leaders, processed, targetLeaders);
                if (leaders.Count >= targetLeaders) break;
            }
        }

        return leaders;
    }

    private async Task<MatrixPositionDto> BuildDtoAsync(int userId, int matrixType)
    {
        var userInfo = await _repo.GetAffiliateByIdAsync(userId, _tenantContext.TenantId);
        var cfg = await _configurationServiceAdapter.GetMatrixConfigurationAsync(_tenantContext.TenantId, matrixType);

        return new MatrixPositionDto
        {
            UserId = userId,
            UserName = userInfo?.Username ?? "Usuario desconocido",
            MatrixType = matrixType,
            MatrixName = cfg?.MatrixName ?? $"Matriz {matrixType}",
            Level = 0,
            CreatedAt = userInfo?.CreatedAt ?? DateTime.UtcNow
        };
    }

    private async Task AddLeaderIfInMatrixAsync(
        int candidateId,
        int matrixType,
        int cycle,
        IList<MatrixPositionDto> leaders,
        ISet<int> processed,
        int max)
    {
        if (leaders.Count >= max) return;

        bool isActive = await _repo.IsUserActiveInMatrixAsync(candidateId, matrixType, cycle);

        if (!isActive) return;
        if (!processed.Add(candidateId)) return;

        leaders.Add(await BuildDtoAsync(candidateId, matrixType));
    }

    private async Task<IEnumerable<int>> GetChildrenIdsAsync(int fatherId)
    {
        var children = await _repo.GetChildrenByFatherId(fatherId, _tenantContext.TenantId);
        return children.Select(c => (int)c.Id);
    }

    private async Task<int> CountActiveChildrenInMatrixAsync(int fatherId, int matrixType)
    {
        return await _repo.CountQualifiedChildrenByMatrixAsync(fatherId, matrixType);
    }
}
