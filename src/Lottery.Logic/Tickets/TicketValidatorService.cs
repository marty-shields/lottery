using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;

namespace Lottery.Logic.Tickets;

public class TicketValidatorService : ITicketValidatorService
{
    private readonly IEnumerable<ITicketValidator> _ticketValidators;

    public TicketValidatorService(IEnumerable<ITicketValidator> ticketValidators)
    {
        _ticketValidators = ticketValidators;
    }

    public TicketValidatorServiceResult ValidatePlayerRequestedTickets(Player player, int ticketCount)
    {
        var results = new List<TicketValidatorResult>();
        foreach (var validator in _ticketValidators)
        {
            var result = validator.Validate(player, ticketCount);
            results.Add(result);
        }

        return new TicketValidatorServiceResult(results);
    }
}
