using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Grading;

public record GetAllGradingsQuery : IRequest<ICollection<GradingDto>>;
