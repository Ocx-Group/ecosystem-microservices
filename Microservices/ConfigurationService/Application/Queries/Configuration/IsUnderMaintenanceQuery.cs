using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Configuration;

public record IsUnderMaintenanceQuery : IRequest<bool>;
