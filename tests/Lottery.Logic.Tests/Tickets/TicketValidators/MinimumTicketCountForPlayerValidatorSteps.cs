using Lottery.Core.Configuration;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;
using Lottery.Logic.Tickets.TicketValidators;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Tickets.TicketValidators;

[Binding]
[Scope(Feature = "Minimum Ticket Count For Player Validator")]
public class MinimumTicketCountForPlayerValidatorSteps
{
    private readonly ScenarioContext _context;
    private readonly Mock<IOptions<TicketConfiguration>> _optionsMock;

    public MinimumTicketCountForPlayerValidatorSteps(ScenarioContext context)
    {
        _context = context;
        _optionsMock = new Mock<IOptions<TicketConfiguration>>();
    }

    [Given("the minimum amount of tickets allowed is {int}")]
    public void GivenMinimumTicketsAllowedIs(int allowedCount)
    {
        _optionsMock.Setup(x => x.Value).Returns(new TicketConfiguration
        {
            MaximumTicketsPerPlayer = 1,
            MinimumTicketsPerPlayer = allowedCount,
            TicketPrice = 1
        });
        _context.Add("MinimumTicketCountForPlayerValidator.MinimumTicketsAllowed", allowedCount);
    }

    [When("I validate the tickets")]
    public void WhenIValidateTickets()
    {
        Core.Entities.Player? player = _context["player"] as Core.Entities.Player;
        IEnumerable<Core.Entities.Ticket>? tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;
        ITicketValidator ticketValidator = new MinimumTicketCountForPlayerValidator(_optionsMock.Object);
        _context.Add("ITicketValidator.Result", ticketValidator.Validate(player!, tickets!.Count()!));
    }

    [Then("the validation should fail with the appropriate error message")]
    public void ThenValidationShouldFail()
    {
        TicketValidatorResult? result = _context["ITicketValidator.Result"] as TicketValidatorResult;
        int? minimumTicketsAllowed = int.Parse(_context["MinimumTicketCountForPlayerValidator.MinimumTicketsAllowed"].ToString()!);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count());
        Assert.AreEqual($"Player must have at least {minimumTicketsAllowed} tickets.", result.Errors.First());
    }
}
