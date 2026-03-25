using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Interfaces;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetAllTicketsByAffiliateIdHandler : IRequestHandler<GetAllTicketsByAffiliateIdQuery, ICollection<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IBrandService _brandService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTicketsByAffiliateIdHandler> _logger;

    public GetAllTicketsByAffiliateIdHandler(
        ITicketRepository ticketRepository,
        IBrandService brandService,
        IMapper mapper,
        ILogger<GetAllTicketsByAffiliateIdHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _brandService = brandService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<TicketDto>> Handle(GetAllTicketsByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _ticketRepository.GetAllTicketsByAffiliateId(request.AffiliateId, _brandService.BrandId);
        return _mapper.Map<ICollection<TicketDto>>(result);
    }
}
