using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;

public record GetAllMatrixConfigurationsQuery : IRequest<IEnumerable<MatrixConfigDto>?>;
