using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class AddLeaderBoardModel5Handler : IRequestHandler<AddLeaderBoardModel5Command, string>
{
    private readonly ILeaderBoardModel5Repository _repository;
    private readonly ILogger<AddLeaderBoardModel5Handler> _logger;

    public AddLeaderBoardModel5Handler(
        ILeaderBoardModel5Repository repository,
        ILogger<AddLeaderBoardModel5Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(AddLeaderBoardModel5Command request, CancellationToken cancellationToken)
    {
        await _repository.AddLeaderBoard(request.LeaderBoard);
        return "Ok";
    }
}
