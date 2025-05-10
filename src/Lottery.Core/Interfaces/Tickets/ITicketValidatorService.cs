using Lottery.Core.Entities;
using Lottery.Core.Results;

namespace Lottery.Core.Interfaces.Tickets;

public interface ITicketValidatorService
{
    /// <summary>
    /// Validates the tickets requested by a player.
    /// </summary>
    /// <param name="player">The player requesting the tickets.</param>
    /// <param name="ticketCount">The ticket count to be validated.</param>
    /// <returns>A result indicating whether the tickets are valid or not.</returns>
    TicketValidatorServiceResult ValidatePlayerRequestedTickets(Player player, int ticketCount);
}
