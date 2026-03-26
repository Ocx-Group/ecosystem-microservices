using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Grading;

public record DeleteGradingCommand(int Id) : IRequest<GradingDto?>;
