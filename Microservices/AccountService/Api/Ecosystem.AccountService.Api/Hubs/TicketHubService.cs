using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Ecosystem.AccountService.Api.Hubs;

public class TicketHubService(IMediator mediator) : Hub
{
    public async Task CreateTicket(CreateTicketCommand command)
    {
        var result = await mediator.Send(command);
        await Clients.Caller.SendAsync("TicketCreated", result);
    }

    public async Task GetAllTicketsByAffiliateId(int affiliateId, int brandId)
    {
        var result = await mediator.Send(new GetAllTicketsByAffiliateIdQuery(affiliateId));
        var ordered = result.OrderByDescending(x => x.CreatedAt).ToList();
        await Clients.Caller.SendAsync("ReceiveTickets", ordered);
    }

    public async Task JoinTicketRoom(int ticketId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Ticket-{ticketId}");
    }

    public async Task LeaveTicketRoom(int ticketId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Ticket-{ticketId}");
    }

    public async Task SendMessageToTicket(SendTicketMessageCommand command)
    {
        await mediator.Send(command);
        await Clients.Group($"Ticket-{command.TicketId}").SendAsync("ReceiveMessage", command);
    }

    public async Task GetAllTickets(int brandId)
    {
        var result = await mediator.Send(new GetAllTicketsQuery(brandId));
        await Clients.Caller.SendAsync("ReceiveTicketsForAdmin", result);
    }

    public async Task DeleteTickets(List<int> ticketIds)
    {
        var result = await mediator.Send(new DeleteTicketsCommand(ticketIds));
        await Clients.Caller.SendAsync("DeleteTicketResponse", result);
    }

    public async Task GetTicketById(int ticketId)
    {
        var result = await mediator.Send(new GetTicketByIdQuery(ticketId));
        await Clients.Caller.SendAsync("GetTicketById", result);
    }

    public async Task GetTicketSummariesByAffiliateId(int affiliateId, int brandId)
    {
        var result = await mediator.Send(new GetTicketSummariesByAffiliateIdQuery(affiliateId, brandId));
        await Clients.Caller.SendAsync("ReceiveTicketSummaries", result);
    }

    public async Task MarkTicketMessagesAsRead(int ticketId)
    {
        var success = await mediator.Send(new MarkTicketMessagesAsReadCommand(ticketId));
        if (success)
            await Clients.Caller.SendAsync("MessagesMarkedAsRead", ticketId);
    }

    public async Task GetAllTicketSummaries(int brandId)
    {
        var result = await mediator.Send(new GetAllTicketSummariesQuery(brandId));
        await Clients.Caller.SendAsync("ReceiveTicketSummaries", result);
    }
}
