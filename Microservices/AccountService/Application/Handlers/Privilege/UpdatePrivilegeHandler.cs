using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Privilege;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Privilege;

public class UpdatePrivilegeHandler : IRequestHandler<UpdatePrivilegeCommand, PrivilegesDto?>
{
    private readonly IPrivilegeRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePrivilegeHandler> _logger;

    public UpdatePrivilegeHandler(
        IPrivilegeRepository repo,
        IMapper mapper,
        ILogger<UpdatePrivilegeHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PrivilegesDto?> Handle(UpdatePrivilegeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.GetPrivilegeByIdAsync(request.Id);
        if (existing is null)
            return null;

        existing.CanCreate = request.CanCreate;
        existing.CanEdit = request.CanEdit;
        existing.CanRead = request.CanRead;
        existing.CanDelete = request.CanDelete;

        var updated = await _repo.UpdatePrivilegeAsync(existing);
        return _mapper.Map<PrivilegesDto>(updated);
    }
}
