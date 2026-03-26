using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;

public record DeletePaymentGroupCommand(int Id) : IRequest<PaymentGroupsDto?>;
