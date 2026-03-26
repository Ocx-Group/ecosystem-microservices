using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Application.Queries.Privilege;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Privilege;

public class GetAllPrivilegesHandler : IRequestHandler<GetAllPrivilegesQuery, ICollection<PrivilegesDto>>
{
    private readonly IPrivilegeRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllPrivilegesHandler> _logger;

    public GetAllPrivilegesHandler(
        IPrivilegeRepository repo,
        IMapper mapper,
        ILogger<GetAllPrivilegesHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<PrivilegesDto>> Handle(GetAllPrivilegesQuery request, CancellationToken cancellationToken)
    {
        var privileges = await _repo.GetPrivilegesAsync();
        return _mapper.Map<ICollection<PrivilegesDto>>(privileges);
    }
}
