using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.PaymentGroup;

public record GetAllPaymentGroupsQuery : IRequest<ICollection<PaymentGroupsDto>>;
