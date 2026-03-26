using Ecosystem.AccountService.Application.DTOs;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Affiliate;

public record UpdateUserProfileCommand(
    int Id,
    string Identification,
    byte BinaryMatrixSide,
    string? Address,
    string Phone,
    string? ZipCode,
    int? Country,
    DateTime? Birthday,
    string? TaxId,
    string? LegalAuthorizedFirst,
    string? LegalAuthorizedSecond,
    string? BeneficiaryName,
    string? BeneficiaryEmail,
    string? BeneficiaryPhone
) : IRequest<UsersAffiliatesDto?>;
