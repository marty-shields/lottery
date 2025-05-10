using Lottery.Core.Results;

using Reqnroll;

namespace Lottery.Logic.Tests.Tickets.TicketValidators;

[Binding]
[Scope(Tag = "ticket-validator")]
[Scope(Feature = "Ticket Validator Service")]
public class TicketValidatorSteps
{
    private readonly ScenarioContext _context;

    public TicketValidatorSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Given("the player has {int} tickets")]
    public void GivenPlayerHasTickets(int ticketCount)
    {
        Core.Entities.Player player = new("Player 1", 10);
        Core.Entities.Ticket[] tickets = new Core.Entities.Ticket[ticketCount];
        for (int i = 0; i < ticketCount; i++)
        {
            tickets[i] = new Core.Entities.Ticket(player.Name);
        }
        _context.Add("player", player);
        _context.Add("tickets", tickets);
    }

    [Then("the validation should succeed")]
    public void ThenValidationSucceeds()
    {
        TicketValidatorResult? result = _context["ITicketValidator.Result"] as TicketValidatorResult;
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual(0, result.Errors.Count());
    }
}
