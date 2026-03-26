using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class AddLeaderBoardModel6Handler : IRequestHandler<AddLeaderBoardModel6Command, string>
{
    private readonly ILeaderBoardModel6Repository _repository;
    private readonly ILogger<AddLeaderBoardModel6Handler> _logger;

    public AddLeaderBoardModel6Handler(
        ILeaderBoardModel6Repository repository,
        ILogger<AddLeaderBoardModel6Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(AddLeaderBoardModel6Command request, CancellationToken cancellationToken)
    {
        await _repository.AddLeaderBoard(request.LeaderBoard);
        return "Ok";
    }
}
