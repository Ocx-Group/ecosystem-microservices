using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetAllTicketsHandler : IRequestHandler<GetAllTicketsQuery, ICollection<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserAffiliateInfoRepository _userAffiliateInfoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTicketsHandler> _logger;

    public GetAllTicketsHandler(
        ITicketRepository ticketRepository,
        IUserAffiliateInfoRepository userAffiliateInfoRepository,
        IMapper mapper,
        ILogger<GetAllTicketsHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _userAffiliateInfoRepository = userAffiliateInfoRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetAllTickets(request.BrandId);

        var userIds = tickets.Select(t => t.AffiliateId).Distinct().ToList();
        var users = await _userAffiliateInfoRepository.GetAffiliatesByIds(userIds.ToArray(), request.BrandId);
        var userDictionary = users.ToDictionary(u => u.Id, u => u.Username);

        var ticketDtos = _mapper.Map<ICollection<TicketDto>>(tickets);
        ticketDtos = ticketDtos.OrderByDescending(x => x.CreatedAt).ToList();

        foreach (var ticketDto in ticketDtos)
        {
            if (userDictionary.TryGetValue(ticketDto.AffiliateId, out var username))
            {
                ticketDto.UserName = username;
            }
        }

        return ticketDtos;
    }
}
