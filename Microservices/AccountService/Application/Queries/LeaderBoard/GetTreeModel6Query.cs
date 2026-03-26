using Ecosystem.AccountService.Domain.Models.CustomModels;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.LeaderBoard;

public record GetTreeModel6Query(int UserId) : IRequest<List<ModelsFamilyTree>>;
