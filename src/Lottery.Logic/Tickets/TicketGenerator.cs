using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Tickets;

public class TicketGenerator : ITicketGenerator
{
    private readonly ITicketValidatorService _ticketValidatorService;
    private readonly TicketConfiguration _ticketConfiguration;

    public TicketGenerator(ITicketValidatorService ticketValidatorService, IOptions<TicketConfiguration> ticketConfiguration)
    {
        _ticketValidatorService = ticketValidatorService;
        _ticketConfiguration = ticketConfiguration.Value;
    }

    public IEnumerable<Ticket> GenerateTicketsForPlayer(Player player, int requestedTicketsCount)
    {
        int adjustedTicketsCount = AdjustRequestTicketsCountForPlayer(player, requestedTicketsCount);

        if (adjustedTicketsCount == 0)
        {
            throw new InvalidOperationException("Player does not have enough balance to generate tickets.");
        }

        Core.Results.TicketValidatorServiceResult validationResult = _ticketValidatorService.ValidatePlayerRequestedTickets(player, adjustedTicketsCount);

        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException("Ticket generation failed due to validation errors.");
        }

        List<Ticket> tickets = [];
        for (int i = 0; i < adjustedTicketsCount; i++)
        {
            Ticket ticket = new(player.Name);
            tickets.Add(ticket);
        }

        return tickets;
    }

    private int AdjustRequestTicketsCountForPlayer(Player player, int requestedTicketsCount)
    {
        decimal playerBalance = player.Balance;
        decimal ticketPrice = _ticketConfiguration.TicketPrice;
        int maxTickets = (int)(playerBalance / ticketPrice);
        return requestedTicketsCount > maxTickets ? maxTickets : requestedTicketsCount;
    }
}
