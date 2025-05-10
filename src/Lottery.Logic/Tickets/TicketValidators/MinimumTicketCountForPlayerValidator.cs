using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Tickets.TicketValidators;

public class MinimumTicketCountForPlayerValidator : ITicketValidator
{
    private readonly int _minimumTicketPerUser;

    public MinimumTicketCountForPlayerValidator(IOptions<TicketConfiguration> options)
    {
        _minimumTicketPerUser = options.Value.MinimumTicketsPerPlayer;
    }

    public TicketValidatorResult Validate(Player player, int ticketCount)
    {
        if (ticketCount < _minimumTicketPerUser)
        {
            return TicketValidatorResult.Invalid($"Player must have at least {_minimumTicketPerUser} tickets.");
        }

        return TicketValidatorResult.Valid();
    }
}
