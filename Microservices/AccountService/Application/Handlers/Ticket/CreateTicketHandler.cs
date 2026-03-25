using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Interfaces;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, TicketDto?>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTicketHandler> _logger;

    public CreateTicketHandler(
        ITicketRepository ticketRepository,
        IBrandService brandService,
        IMapper mapper,
        ILogger<CreateTicketHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _brandService = brandService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TicketDto?> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map<Domain.Models.Ticket>(request);
        var result = await _ticketRepository.CreateTicket(ticket, _brandService.BrandId);

        return result is null ? null : _mapper.Map<TicketDto>(result);
    }
}
