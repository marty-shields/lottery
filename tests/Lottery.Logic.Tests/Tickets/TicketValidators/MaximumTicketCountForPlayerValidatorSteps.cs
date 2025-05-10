using Lottery.Core.Configuration;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;
using Lottery.Logic.Tickets.TicketValidators;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Tickets.TicketValidators;

[Binding]
[Scope(Feature = "Maximum Ticket Count For Player Validator")]
public class MaximumTicketCountForPlayerValidatorSteps
{
    private readonly ScenarioContext _context;
    private readonly Mock<IOptions<TicketConfiguration>> _optionsMock;

    public MaximumTicketCountForPlayerValidatorSteps(ScenarioContext context)
    {
        _context = context;
        _optionsMock = new Mock<IOptions<TicketConfiguration>>();
    }

    [Given("the maximum amount of tickets allowed is {int}")]
    public void GivenMaximumTicketsAllowedIs(int allowedCount)
    {
        _optionsMock.Setup(x => x.Value).Returns(new TicketConfiguration
        {
            MaximumTicketsPerPlayer = allowedCount,
            MinimumTicketsPerPlayer = 0,
            TicketPrice = 1
        });
        _context.Add("MaximumTicketCountForPlayerValidator.MaximumTicketsAllowed", allowedCount);
    }

    [When("I validate the tickets")]
    public void WhenIValidateTickets()
    {
        Core.Entities.Player? player = _context["player"] as Core.Entities.Player;
        IEnumerable<Core.Entities.Ticket>? tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;
        ITicketValidator ticketValidator = new MaximumTicketCountForPlayerValidator(_optionsMock.Object);
        _context.Add("ITicketValidator.Result", ticketValidator.Validate(player!, tickets!.Count()));
    }

    [Then("the validation should fail with the appropriate error message")]
    public void ThenValidationShouldFail()
    {
        TicketValidatorResult? result = _context["ITicketValidator.Result"] as TicketValidatorResult;
        int? maximumTicketsAllowed = int.Parse(_context["MaximumTicketCountForPlayerValidator.MaximumTicketsAllowed"].ToString()!);
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(1, result.Errors.Count());
        Assert.AreEqual($"Player must not have more than {maximumTicketsAllowed} tickets.", result.Errors.First());
    }
}
