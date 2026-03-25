using Ecosystem.AccountService.Application.Commands.Auth;
using Ecosystem.AccountService.Application.Helpers;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class GenerateHashHandler : IRequestHandler<GenerateHashCommand, string>
{
    public Task<string> Handle(GenerateHashCommand request, CancellationToken cancellationToken)
        => Task.FromResult(PasswordHelper.HashPassword(request.Password));
}
