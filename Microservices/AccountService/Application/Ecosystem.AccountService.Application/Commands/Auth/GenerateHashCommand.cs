using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Auth;

public record GenerateHashCommand(string Password) : IRequest<string>;
