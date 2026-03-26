using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class DeleteLeaderBoardModel6Handler : IRequestHandler<DeleteLeaderBoardModel6Command, string>
{
    private readonly ILeaderBoardModel6Repository _repository;
    private readonly ILogger<DeleteLeaderBoardModel6Handler> _logger;

    public DeleteLeaderBoardModel6Handler(
        ILeaderBoardModel6Repository repository,
        ILogger<DeleteLeaderBoardModel6Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteLeaderBoardModel6Command request, CancellationToken cancellationToken)
    {
        await _repository.DeleteLeaderBoard();
        return "Ok";
    }
}
