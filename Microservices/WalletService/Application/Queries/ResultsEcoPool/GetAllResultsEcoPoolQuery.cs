using Ecosystem.WalletService.Domain.DTOs.ResultsEcoPoolDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.ResultsEcoPool;

public record GetAllResultsEcoPoolQuery : IRequest<ICollection<ResultsEcoPoolDto>>;
