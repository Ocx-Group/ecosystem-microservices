using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateAffiliateCommand(
    int Id,
    string? Identification,
    string? Name,
    string? UserName,
    string? LastName,
    string? Address,
    string? LegalAuthorizedFirst,
    string? LegalAuthorizedSecond,
    string? Phone,
    string? Email,
    string? ZipCode,
    int? Country,
    string? StatePlace,
    string? City,
    DateTime? Birthday,
    string? TaxId,
    string? BeneficiaryName,
    byte? Status,
    string? AffiliateType,
    int? Father,
    int? Sponsor,
    bool TermsConditions,
    string? BeneficiaryEmail,
    string? BeneficiaryPhone
) : IRequest<UsersAffiliatesDto?>;
