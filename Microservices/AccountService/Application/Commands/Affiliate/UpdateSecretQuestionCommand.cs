using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateSecretQuestionCommand(int Id, string Password, string SecretQuestion, string SecretAnswer) : IRequest<UsersAffiliatesDto?>;
