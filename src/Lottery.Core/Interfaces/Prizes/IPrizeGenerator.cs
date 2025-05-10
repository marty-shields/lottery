using Lottery.Core.Entities;

namespace Lottery.Core.Interfaces.Prizes;

public interface IPrizeGenerator
{
    /// <summary>
    /// Generates prizes for the given tickets.
    /// </summary>
    /// <param name="tickets">The tickets to generate prizes from.</param>
    Prize GeneratePrizes(IEnumerable<Ticket> tickets);
}
