using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Matrix;

public record WhatUsersHave2ChildrenQuery(long[] Users) : IRequest<long[]>;
