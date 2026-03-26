using Ecosystem.AccountService.Application.Commands.LeaderBoard;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.LeaderBoard;

public class DeleteLeaderBoardModel4Handler : IRequestHandler<DeleteLeaderBoardModel4Command, string>
{
    private readonly ILeaderBoardModel4Repository _repository;
    private readonly ILogger<DeleteLeaderBoardModel4Handler> _logger;

    public DeleteLeaderBoardModel4Handler(
        ILeaderBoardModel4Repository repository,
        ILogger<DeleteLeaderBoardModel4Handler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteLeaderBoardModel4Command request, CancellationToken cancellationToken)
    {
        await _repository.DeleteLeaderBoard();
        return "Ok";
    }
}
