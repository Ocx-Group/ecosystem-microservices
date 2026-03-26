using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record CreateAffiliateCommand(
    string UserName,
    string? Name,
    string Password,
    string? LastName,
    string Email,
    int Country,
    string? AffiliateType,
    int Father,
    int? Sponsor,
    int? BinarySponsor,
    string? Phone,
    string? StatePlace,
    string? City,
    byte? BinaryMatrixSide,
    byte? Status,
    bool TermsConditions
) : IRequest<ServicesResponse>;
