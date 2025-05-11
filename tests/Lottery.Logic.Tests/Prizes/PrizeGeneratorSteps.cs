
using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;
using Lottery.Logic.Prizes;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Prizes;

[Binding]
[Scope(Feature = "Prize Generator")]
public class PrizeGeneratorSteps
{
    private List<Ticket>? _tickets;
    private Mock<IOptions<TicketConfiguration>>? _ticketConfiguration;
    private Mock<IOptions<PrizeConfiguration>>? _prizeConfiguration;
    private Mock<IRandomNumberGenerator>? _randomNumberGeneratorMock;
    private Prize? _prize;
    private int _secondTierWinners;

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

    [When("I generate the prizes")]
    public void WhenIGenerateThePrizes()
    {
        IPrizeGenerator prizeGenerator = new PrizeGenerator(_randomNumberGeneratorMock!.Object, _ticketConfiguration!.Object, _prizeConfiguration!.Object);
        _prize = prizeGenerator.GeneratePrizes(_tickets!);
    }

    [Then("I should have 1 grand prize winner that won {float}")]
    public void ThenIShouldHave1GrandPrizeWinnerThatWon(decimal prizeAmount)
    {
        Assert.IsNotNull(_prize);
        Assert.AreEqual(_tickets![0], _prize.GrandPrizeTicket);
        Assert.AreEqual(prizeAmount, _prize.GrandPrizeAmount);
    }

    [Then("I should have {int} second tier winner that won {float}")]
    public void ThenIShouldHaveSecondTierWinnerThatWon(int winnerCount, decimal prizeAmount)
    {
        Assert.IsNotNull(_prize);
        Assert.AreEqual(winnerCount, _prize.SecondTierPrizeTickets.Count());
        for (int i = 0; i < winnerCount; i++)
        {
            Assert.AreEqual(_tickets![i + 1], _prize.SecondTierPrizeTickets.ElementAt(i));
        }
        Assert.AreEqual(prizeAmount, _prize.SecondTierPrizeAmount);
        _secondTierWinners = winnerCount;
    }

    [Then("I should have {int} third tier winners that won {float}")]
    public void ThenIShouldHaveThirdTierWinnersThatWon(int winnerCount, decimal prizeAmount)
    {
        Assert.IsNotNull(_prize);
        Assert.AreEqual(winnerCount, _prize.ThirdTierPrizeTickets.Count());
        for (int i = 0; i < winnerCount; i++)
        {
            Assert.AreEqual(_tickets![i + _secondTierWinners + 1], _prize.ThirdTierPrizeTickets.ElementAt(i));
        }
        Assert.AreEqual(prizeAmount, _prize.ThirdTierPrizeAmount);
    }

    [Then("the house profit should be {float}")]
    public void ThenTheHouseProfitShouldBe(decimal profit)
    {
        Assert.IsNotNull(_prize);
        Assert.AreEqual(profit, _prize.HouseProfit);
    }
}
