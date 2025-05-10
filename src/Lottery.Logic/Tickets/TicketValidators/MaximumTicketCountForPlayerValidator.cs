using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Tickets.TicketValidators;

public class MaximumTicketCountForPlayerValidator : ITicketValidator
{
    private readonly int _maximumTicketPerUser;

    public MaximumTicketCountForPlayerValidator(IOptions<TicketConfiguration> options)
    {
        _maximumTicketPerUser = options.Value.MaximumTicketsPerPlayer;
    }

    public TicketValidatorResult Validate(Player player, int ticketCount)
    {
        return ticketCount > _maximumTicketPerUser
            ? TicketValidatorResult.Invalid($"Player must not have more than {_maximumTicketPerUser} tickets.")
            : TicketValidatorResult.Valid();
    }
}
