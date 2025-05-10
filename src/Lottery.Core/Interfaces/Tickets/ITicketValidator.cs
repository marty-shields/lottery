using Lottery.Core.Entities;
using Lottery.Core.Results;

namespace Lottery.Core.Interfaces.Tickets;

public interface ITicketValidator
{
    /// <summary>
    /// Validates that the tickets requested for the player are valid
    /// </summary>
    /// <param name="player">Player to validate</param>
    /// <param name="ticketCount">Count of tickets requested</param>
    /// <returns>Validation result. This will include if the validation was successful and if not, then the errors returned.</returns>
    TicketValidatorResult Validate(Player player, int ticketCount);
}
