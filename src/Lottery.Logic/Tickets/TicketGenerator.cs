using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Tickets;

public class TicketGenerator : ITicketGenerator
{
    private readonly ITicketValidatorService _ticketValidatorService;
    private readonly IOptions<TicketConfiguration> _ticketConfiguration;

    public TicketGenerator(ITicketValidatorService ticketValidatorService, IOptions<TicketConfiguration> ticketConfiguration)
    {
        _ticketValidatorService = ticketValidatorService;
        _ticketConfiguration = ticketConfiguration;
    }

    public IEnumerable<Ticket> GenerateTicketsForPlayer(Player player, int requestedTicketsCount)
    {
        var adjustedTicketsCount = AdjustRequestTicketsCountForPlayer(player, requestedTicketsCount);

        if (adjustedTicketsCount == 0)
        {
            throw new InvalidOperationException("Player does not have enough balance to generate tickets.");
        }

        var validationResult = _ticketValidatorService.ValidatePlayerRequestedTickets(player, adjustedTicketsCount);

        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException("Ticket generation failed due to validation errors.");
        }

        var tickets = new List<Ticket>();
        for (var i = 0; i < adjustedTicketsCount; i++)
        {
            var ticket = new Ticket(player.Name);
            tickets.Add(ticket);
        }

        return tickets;
    }

    private int AdjustRequestTicketsCountForPlayer(Player player, int requestedTicketsCount)
    {
        var playerBalance = player.Balance;
        var ticketPrice = _ticketConfiguration.Value.TicketPrice;
        var maxTickets = (int)(playerBalance / ticketPrice);
        return requestedTicketsCount > maxTickets ? maxTickets : requestedTicketsCount;
    }
}
