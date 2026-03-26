using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.AffiliateAddress;

public record CreateAffiliateAddressCommand(
    int AffiliateId,
    string? FiscalIdentification,
    string AddressName,
    string Name,
    string LastName,
    string? Company,
    string? IvaNumber,
    string Address,
    string? AddressLine2,
    string? PostalCode,
    string? City,
    string? State,
    int Country,
    string LandlinePhone,
    string MobilePhone,
    string? Other,
    DateTime Date,
    string CountryName,
    string? StateName,
    string Email
) : IRequest<AffiliateAddressDto?>;
