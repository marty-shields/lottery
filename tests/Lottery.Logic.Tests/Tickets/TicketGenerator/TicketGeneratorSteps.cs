


using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Tickets;
using Lottery.Core.Results;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Tickets;

[Binding]
[Scope(Feature = "Ticket Generator")]
public class TicketGeneratorSteps
{
    private Player? _player;
    private int _ticketCount;
    private Mock<IOptions<TicketConfiguration>>? _ticketOptionsMock;
    private Mock<ITicketValidatorService>? _validatorServiceMock;
    private IEnumerable<Ticket>? _ticketsGenerated;
    private Exception? _exceptionThrown;

    [Given("the player has a balance of {decimal} and is requesting {int} tickets")]
    public void GivenThePlayerHasABalanceOf(decimal balance, int ticketCount)
    {
        _player = new Player("TestPlayer", balance);
        _ticketCount = ticketCount;
    }

    [Given("the validation allows between {int} and {int} tickets with a ticket price of {int} with {int} tickets")]
    public void GivenTheValidationAllowsBetweenAndTicketsWithATicketPriceOf(int minTickets, int maxTickets, int ticketPrice, int ticketCount)
    {
        _ticketOptionsMock = new Mock<IOptions<TicketConfiguration>>();
        _ticketOptionsMock.SetupGet(x => x.Value).Returns(new TicketConfiguration
        {
            MinimumTicketsPerPlayer = minTickets,
            MaximumTicketsPerPlayer = maxTickets,
            TicketPrice = ticketPrice
        });

        _validatorServiceMock = new Mock<ITicketValidatorService>();
        _validatorServiceMock.Setup(x => x.ValidatePlayerRequestedTickets(_player!, ticketCount))
            .Returns(new TicketValidatorServiceResult(
            [
                TicketValidatorResult.Valid()
            ]));
    }

    [Given("the validation fails with between {int} and {int} tickets with a ticket price of {int} with {int} tickets")]
    public void GivenTheValidationFailsWithBetweenAndTicketsWithATicketPriceOf(int minTickets, int maxTickets, int ticketPrice, int ticketCount)
    {
        _ticketOptionsMock = new Mock<IOptions<TicketConfiguration>>();
        _ticketOptionsMock.SetupGet(x => x.Value).Returns(new TicketConfiguration
        {
            MinimumTicketsPerPlayer = minTickets,
            MaximumTicketsPerPlayer = maxTickets,
            TicketPrice = ticketPrice
        });

        _validatorServiceMock = new Mock<ITicketValidatorService>();
        _validatorServiceMock.Setup(x => x.ValidatePlayerRequestedTickets(_player!, ticketCount))
            .Returns(new TicketValidatorServiceResult(
            [
                TicketValidatorResult.Invalid("Validation failed")
            ]));
    }

    [When("the player generates the tickets requested")]
    public void WhenThePlayerGeneratesTheTicketsRequested()
    {
        ITicketGenerator ticketGenerator = new Logic.Tickets.TicketGenerator(
            _validatorServiceMock!.Object,
            _ticketOptionsMock!.Object
        );

        try
        {
            _ticketsGenerated = ticketGenerator.GenerateTicketsForPlayer(_player!, _ticketCount);
        }
        catch (Exception ex)
        {
            _exceptionThrown = ex;
        }
    }

    [Then("the maximum balance should have been checked {int} times")]
    public void ThenTheMaximumBalanceShouldHaveBeenChecked(int times)
    {
        _ticketOptionsMock!.VerifyGet(x => x.Value, Times.Exactly(times));
    }

    [Then("the validation should have been called with the ticket count of {int}")]
    public void ThenTheValidationShouldHaveBeenCalledWithTheTicketCountOf(int ticketCount)
    {
        _validatorServiceMock!.Verify(x => x.ValidatePlayerRequestedTickets(_player!, ticketCount), Times.Once);
    }

    [Then("the validation should not have been called")]
    public void ThenTheValidationShouldNotHaveBeenCalledWithThePlayer()
    {
        _validatorServiceMock!.Verify(x => x.ValidatePlayerRequestedTickets(It.IsAny<Player>(), It.IsAny<int>()), Times.Never);
    }

    [Then("{int} tickets should be generated successfully")]
    public void ThenTheTicketsShouldBeGeneratedSuccessfully(int ticketCount)
    {
        Assert.IsNotNull(_ticketsGenerated);
        Assert.AreEqual(ticketCount, _ticketsGenerated!.Count());
    }

    [Then("an exception should be thrown of type {string} and Message {string}")]
    public void ThenAnExceptionShouldBeThrownOfTypeAndMessage(string exceptionType, string message)
    {
        Assert.IsNotNull(_exceptionThrown);
        Assert.AreEqual(exceptionType, _exceptionThrown!.GetType().Name);
        Assert.AreEqual(message, _exceptionThrown.Message);
    }
}
