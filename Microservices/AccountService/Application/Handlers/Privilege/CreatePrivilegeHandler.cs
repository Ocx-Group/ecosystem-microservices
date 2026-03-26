using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Privilege;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Privilege;

public class CreatePrivilegeHandler : IRequestHandler<CreatePrivilegeCommand, PrivilegesDto?>
{
    private readonly IPrivilegeRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePrivilegeHandler> _logger;

    public CreatePrivilegeHandler(
        IPrivilegeRepository repo,
        IMapper mapper,
        ILogger<CreatePrivilegeHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PrivilegesDto?> Handle(CreatePrivilegeCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Models.Privilege>(request);
        var created = await _repo.CreatePrivilegeAsync(entity);
        return _mapper.Map<PrivilegesDto>(created);
    }
}
