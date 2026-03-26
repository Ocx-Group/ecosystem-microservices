using Ecosystem.AccountService.Application.DTOs.Matrix;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Matrix;

public record GetUniLevelFamilyTreeQuery(int? UserId) : IRequest<UserUniLevelTreeDto?>;
