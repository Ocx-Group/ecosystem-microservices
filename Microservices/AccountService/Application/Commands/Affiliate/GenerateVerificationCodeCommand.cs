using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record GenerateVerificationCodeCommand(int Id, bool CheckDate) : IRequest<bool>;
