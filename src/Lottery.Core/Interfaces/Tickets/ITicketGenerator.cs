using Lottery.Core.Entities;

namespace Lottery.Core.Interfaces.Tickets;

public interface ITicketGenerator
{
    /// <summary>
    /// This will generate the specified amount of tickets requested for the player.
    /// If the player attempts to request more tickets than their balance then this will be adjusted to allow them to purchase
    /// only the amount they can afford. This adjusted amount will be validated then the tickets will be generated.
    /// </summary>
    /// <param name="player">The player requesting the tickets</param>
    /// <param name="requestedTicketsCount">The amount of tickets the player requested</param>
    /// <returns>A list of tickets generated for the player.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the player is unable to purchase the tickets requested</exception>
    IEnumerable<Ticket> GenerateTicketsForPlayer(Player player, int requestedTicketsCount);
}
