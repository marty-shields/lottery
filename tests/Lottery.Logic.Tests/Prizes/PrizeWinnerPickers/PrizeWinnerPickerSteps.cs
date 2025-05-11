
using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;
using Lottery.Logic.Prizes.WinnerPickers;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Prizes.PrizeWinnerPickers;

[Binding]
[Scope(Feature = "Prize Winner Pickers")]
public class PrizeWinnerPickerSteps
{
    private List<Ticket>? _tickets;
    private Mock<IOptions<TicketConfiguration>>? _ticketConfiguration;
    private Mock<IOptions<PrizeConfiguration>>? _prizeConfiguration;
    private Mock<IRandomNumberGenerator>? _randomNumberGeneratorMock;
    private (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) _prize;

    [Given("the lottery has {int} tickets sold")]
    public void GivenTheLotteryHasTicketsSold(int ticketsSold)
    {
        _tickets = [];
        for (int i = 0; i < ticketsSold; i++)
        {
            _tickets.Add(new Ticket("Player" + i));
        }
    }

    [Given("the price of each ticket was {int}")]
    public void GivenThePriceOfEachTicketWas(int ticketPrice)
    {
        _ticketConfiguration = new Mock<IOptions<TicketConfiguration>>();
        _ticketConfiguration.Setup(x => x.Value).Returns(new TicketConfiguration
        {
            MaximumTicketsPerPlayer = 10,
            MinimumTicketsPerPlayer = 1,
            TicketPrice = ticketPrice
        });
    }

    [Given("the prize payouts are set to 50% for the grand prize, 30% for the second tier, and 10% for the third tier")]
    public void GivenThePrizePayoutsAreSetTo50ForTheGrandPrize30ForTheSecondTierAnd10ForTheThirdTier()
    {
        PrizeConfiguration prizeConfiguration = new()
        {
            GrandPrizePayoutPercentage = 0.5m,
            SecondPrizePayoutPercentage = 0.3m,
            ThirdPrizePayoutPercentage = 0.1m,
            SecondPrizeWinnerCountPercentage = 0.1m,
            ThirdPrizeWinnerCountPercentage = 0.2m
        };
        _prizeConfiguration = new Mock<IOptions<PrizeConfiguration>>();
        _prizeConfiguration.Setup(x => x.Value).Returns(prizeConfiguration);
    }

    [Given("the random number generator is setup to always return 0")]
    public void GivenTheRandomNumberGeneratorIsSetupToAlwaysReturn0()
    {
        _randomNumberGeneratorMock = new Mock<IRandomNumberGenerator>();
        _randomNumberGeneratorMock.Setup(x => x.GenerateNewRandomNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
    }

    [When("I run the grand prize picker")]
    public void WhenIRunGrandPrizePicker()
    {
        IPrizeWinnerPicker picker = new GrandPrizeWinnerPicker(_randomNumberGeneratorMock!.Object, _prizeConfiguration!.Object, _ticketConfiguration!.Object);
        _prize = picker.CalculateWinners(_tickets!, _tickets!);
    }

    [When("I run the second tier prize picker")]
    public void WhenIRunSecondTierPicker()
    {
        IPrizeWinnerPicker picker = new SecondTierWinnerPicker(_randomNumberGeneratorMock!.Object, _prizeConfiguration!.Object, _ticketConfiguration!.Object);
        _prize = picker.CalculateWinners(_tickets!, _tickets!);
    }

    [When("I run the third tier prize picker")]
    public void WhenIRunThirdTierPicker()
    {
        IPrizeWinnerPicker picker = new ThirdTierWinnerPicker(_randomNumberGeneratorMock!.Object, _prizeConfiguration!.Object, _ticketConfiguration!.Object);
        _prize = picker.CalculateWinners(_tickets!, _tickets!);
    }

    [Then("I should have 1 grand prize winner that won {float}")]
    public void ThenIShouldHave1GrandPrizeWinnerThatWon(decimal prizeAmount)
    {
        Assert.IsNotNull(_prize.prizeWinners);
        Assert.IsNotNull(_prize.prizeAmount);
        Assert.AreEqual(1, _prize.prizeWinners.Count());
        Assert.AreEqual(_tickets![0], _prize.prizeWinners.First());
        Assert.AreEqual(prizeAmount, _prize.prizeAmount);
    }

    [Then("I should have {int} second tier winner that won {float}")]
    public void ThenIShouldHaveSecondTierWinnerThatWon(int winnerCount, decimal prizeAmount)
    {
        Assert.IsNotNull(_prize.prizeWinners);
        Assert.IsNotNull(_prize.prizeAmount);
        Assert.AreEqual(winnerCount, _prize.prizeWinners.Count());
        for (int i = 0; i < winnerCount; i++)
        {
            Assert.AreEqual(_tickets![i], _prize.prizeWinners.ElementAt(i));
        }
        Assert.AreEqual(prizeAmount, _prize.prizeAmount);
    }

    [Then("I should have {int} third tier winners that won {float}")]
    public void ThenIShouldHaveThirdTierWinnersThatWon(int winnerCount, decimal prizeAmount)
    {
        Assert.IsNotNull(_prize.prizeWinners);
        Assert.IsNotNull(_prize.prizeAmount);
        Assert.AreEqual(winnerCount, _prize.prizeWinners.Count());
        for (int i = 0; i < winnerCount; i++)
        {
            Assert.AreEqual(_tickets![i], _prize.prizeWinners.ElementAt(i));
        }
        Assert.AreEqual(prizeAmount, _prize.prizeAmount);
    }

}