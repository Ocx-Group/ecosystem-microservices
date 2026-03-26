using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class DeleteLeaderBoardModel5Handler : IRequestHandler<DeleteLeaderBoardModel5Command, string>
{
    private readonly ILeaderBoardModel5Repository _repository;
    private readonly ILogger<DeleteLeaderBoardModel5Handler> _logger;

    public DeleteLeaderBoardModel5Handler(
        ILeaderBoardModel5Repository repository,
        ILogger<DeleteLeaderBoardModel5Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteLeaderBoardModel5Command request, CancellationToken cancellationToken)
    {
        await _repository.DeleteLeaderBoard();
        return "Ok";
    }
}
