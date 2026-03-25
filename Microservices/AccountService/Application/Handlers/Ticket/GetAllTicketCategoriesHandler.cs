using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetAllTicketCategoriesHandler : IRequestHandler<GetAllTicketCategoriesQuery, ICollection<TicketCategoriesDto>>
{
    private readonly ITicketCategoriesRepository _ticketCategoriesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTicketCategoriesHandler> _logger;

    public GetAllTicketCategoriesHandler(
        ITicketCategoriesRepository ticketCategoriesRepository,
        IMapper mapper,
        ILogger<GetAllTicketCategoriesHandler> logger)
    {
        _ticketCategoriesRepository = ticketCategoriesRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<TicketCategoriesDto>> Handle(GetAllTicketCategoriesQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketCategoriesRepository.GetAllTicketsCategories();
        return _mapper.Map<ICollection<TicketCategoriesDto>>(tickets);
    }
}
