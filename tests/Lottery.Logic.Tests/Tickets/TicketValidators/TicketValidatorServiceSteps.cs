using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;
using Lottery.Logic.Tickets;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Tickets.TicketValidators;

[Binding]
[Scope(Feature = "Ticket Validator Service")]
public class TicketValidatorServiceSteps
{
    private readonly ScenarioContext _context;

    public TicketValidatorServiceSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Given("there are validators that would mark the tickets as valid")]
    public void GivenValidatorsMarkTicketsAsValid()
    {
        var player = _context["player"] as Core.Entities.Player;
        var tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;

        var ticketValidatorMockList = new List<Mock<ITicketValidator>>();
        for (int i = 0; i < 3; i++)
        {
            var ticketValidatorMock = new Mock<ITicketValidator>();
            ticketValidatorMock.Setup(v => v.Validate(player!, tickets!.Count()))
                .Returns(new TicketValidatorResult(true, Enumerable.Empty<string>()));
            ticketValidatorMockList.Add(ticketValidatorMock);
        }
        _context.Add("ITicketValidatorMockList", ticketValidatorMockList);
    }

    [Given("there are validators that would mark the tickets as invalid")]
    public void GivenValidatorsMarkTicketsAsInvalid()
    {
        var player = _context["player"] as Core.Entities.Player;
        var tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;

        var ticketValidatorMockList = new List<Mock<ITicketValidator>>();
        var ticketValidatorMockErrorList = new List<Mock<ITicketValidator>>();
        for (int i = 0; i < 3; i++)
        {
            var ticketValidatorMockError = new Mock<ITicketValidator>();
            ticketValidatorMockError.Setup(v => v.Validate(player!, tickets!.Count()))
                .Returns(TicketValidatorResult.Invalid(new[] { $"Error{i}" }));
            ticketValidatorMockList.Add(ticketValidatorMockError);
            ticketValidatorMockErrorList.Add(ticketValidatorMockError);

            var ticketValidatorMock = new Mock<ITicketValidator>();
            ticketValidatorMock.Setup(v => v.Validate(player!, tickets!.Count()))
                .Returns(TicketValidatorResult.Valid());
            ticketValidatorMockList.Add(ticketValidatorMock);
        }
        _context.Add("ITicketValidatorMockList", ticketValidatorMockList);
        _context.Add("ITicketValidatorMockErrorList", ticketValidatorMockErrorList);
    }

    [When("the player tries to validate the tickets")]
    public void WhenPlayerTriesToValidateTickets()
    {
        var player = _context["player"] as Core.Entities.Player;
        var tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;
        var ticketValidatorMockList = _context["ITicketValidatorMockList"] as List<Mock<ITicketValidator>>;

        var ticketValidatorService = new TicketValidatorService(ticketValidatorMockList!.Select(v => v.Object));
        var result = ticketValidatorService.ValidatePlayerRequestedTickets(player!, tickets!.Count());

        _context.Add("result", result);
    }

    [Then("the result should come back as invalid with errors")]
    public void ThenResultShouldBeInvalidWithErrors()
    {
        var result = _context["result"] as TicketValidatorServiceResult;

        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsValid);

        var ticketValidatorMockErrorList = _context["ITicketValidatorMockErrorList"] as List<Mock<ITicketValidator>>;
        Assert.AreEqual(ticketValidatorMockErrorList!.Count(), result.Errors.Count());
        for (int i = 0; i < result.Errors.Count(); i++)
        {
            var error = $"Error{i}";
            Assert.IsTrue(result.Errors.Contains(error));
        }
    }

    [Then("the result should come back as valid with no errors")]
    public void ThenResultShouldBeValidWithNoErrors()
    {
        var result = _context["result"] as TicketValidatorServiceResult;

        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual(0, result.Errors.Count());
    }

    [Then("each validator should be called once")]
    public void GivenEachValidatorShouldBeCalledOnce()
    {
        var player = _context["player"] as Core.Entities.Player;
        var tickets = _context["tickets"] as IEnumerable<Core.Entities.Ticket>;
        var ticketValidatorMockList = _context["ITicketValidatorMockList"] as List<Mock<ITicketValidator>>;

        foreach (var validatorMock in ticketValidatorMockList!)
        {
            validatorMock.Verify(v => v.Validate(player!, tickets!.Count()), Times.Once);
            validatorMock.Verify(v => v.Validate(It.IsAny<Core.Entities.Player>(), It.IsAny<int>()), Times.Once);
        }
    }
}
