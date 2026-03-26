using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateCardIdImageCommand(int Id, string CardIdImage, string CardIdMessage) : IRequest<UsersAffiliatesDto?>;
