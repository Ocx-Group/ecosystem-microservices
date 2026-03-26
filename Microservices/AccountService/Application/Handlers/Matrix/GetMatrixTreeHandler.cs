using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Application.Queries.Matrix;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Matrix;

public class GetMatrixTreeHandler : IRequestHandler<GetMatrixTreeQuery, MatrixDto?>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMatrixTreeHandler> _logger;

    public GetMatrixTreeHandler(
        IUserAffiliateInfoRepository repo,
        IMapper mapper,
        ILogger<GetMatrixTreeHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatrixDto?> Handle(GetMatrixTreeQuery request, CancellationToken cancellationToken)
    {
        var isAdmin = request.UserId == 0;
        var rootId = isAdmin ? 0 : request.UserId;
        var isAdminFlag = (byte)(isAdmin ? 1 : 0);

        var nodes = await _repo.GetMatrixFamilyTreeByMatrixType(
            maxLevels: 7,
            isAdmin: isAdminFlag,
            matrixType: request.MatrixType,
            id: rootId);

        if (nodes.Count == 0)
            return null;

        var dtoList = _mapper.Map<ICollection<MatrixDto>>(nodes);
        var tree = ConvertListToTreeMatrix(dtoList, rootId, isAdmin);

        if (isAdmin)
        {
            return new MatrixDto
            {
                UserId = 0,
                Username = "Administrador",
                Children = new List<MatrixDto> { tree }
            };
        }

        return tree;
    }

    private static MatrixDto ConvertListToTreeMatrix(ICollection<MatrixDto> list, int id, bool isAdmin)
    {
        var lookup = list.ToLookup(x => x.Father);
        foreach (var item in list)
            item.Children = lookup[item.UserId].ToList();

        var root = isAdmin
            ? list.FirstOrDefault(x => x.UserId == 0)
            : list.FirstOrDefault(x => x.UserId == id);

        if (root == null)
            throw new InvalidOperationException(
                $"No root node found. isAdmin={isAdmin}, expected id={id}. Actual list: {string.Join(",", list.Select(x => x.UserId))}");

        return root;
    }
}
