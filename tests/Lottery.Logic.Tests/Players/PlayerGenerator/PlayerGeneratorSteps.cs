using Lottery.Core.Configuration;
using Lottery.Core.Interfaces.Players;
using Lottery.Logic.Players;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Players;

[Binding]
[Scope(Feature = "Player Generator")]
public class PlayerGeneratorSteps
{
    private readonly Mock<IOptions<PlayerConfiguration>> _options;
    private Core.Entities.Player? actualPlayer;
    private int playerBalance;
    private string? playerName;

    public PlayerGeneratorSteps()
    {
        _options = new Mock<IOptions<PlayerConfiguration>>();
    }

    [Given("I have a player name {string}")]
    public void GivenAPlayerWithName(string name)
    {
        playerName = name;
    }

    [Given("I have a starting balance setup in configuration as {int}")]
    public void GivenAPlayerWithName(int balance)
    {
        playerBalance = balance;
        _options.Setup(o => o.Value).Returns(new PlayerConfiguration
        {
            MinimumPlayersAllowed = 0,
            MaximumPlayersAllowed = 0,
            StartingBalance = playerBalance
        });
    }

    [When("the player is generated")]
    public void WhenThePlayerIsGenerated()
    {
        // Logic to generate the player can be added here
        IPlayerGenerator playerGenerator = new Logic.Players.PlayerGenerator(_options.Object);
        actualPlayer = playerGenerator.GenerateNewPlayer(playerName!);
    }

    [Then("the player should be created successfully")]
    public void ThenPlayerShouldBeCreated()
    {
        Assert.IsNotNull(actualPlayer, "Player should not be null");
    }

    [Then("the player name should be {string}")]
    public void ThenPlayerNameShouldBe(string expectedName)
    {
        Assert.AreEqual(expectedName, actualPlayer!.Name, $"Expected player name: {expectedName}, but got: {actualPlayer.Name}");
    }

    [Then("the player balance should be {int}")]
    public void ThenPlayerBalanceShouldBe(int expectedBalance)
    {
        Assert.AreEqual(expectedBalance, actualPlayer!.Balance, $"Expected player balance: {expectedBalance}, but got: {actualPlayer.Balance}");
    }
}