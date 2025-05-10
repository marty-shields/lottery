
using Lottery.Core.Configuration;
using Lottery.Core.Interfaces.Players;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

using Moq;

using Reqnroll;

namespace Lottery.Logic.Tests.Players;

[Binding]
[Scope(Feature = "Player Factory")]
public class PlayerFactorySteps
{
    private readonly Mock<IOptions<PlayerConfiguration>> _optionsMock;
    private readonly Mock<IRandomNumberGenerator> _numberGeneratorMock;
    private readonly Mock<IPlayerGenerator> _playerGeneratorMock;
    private Core.Entities.Player[]? _expectedComputerPlayersGenerated;
    private Core.Entities.Player? _expectedHumanPlayer;
    private IEnumerable<Core.Entities.Player>? _actualComputerPlayers;
    private Core.Entities.Player? _actualHumanPlayer;

    public PlayerFactorySteps()
    {
        _optionsMock = new Mock<IOptions<PlayerConfiguration>>();
        _numberGeneratorMock = new Mock<IRandomNumberGenerator>();
        _playerGeneratorMock = new Mock<IPlayerGenerator>();
    }

    [Given("there is configuration that sets the min computer players to {int} and max players to {int}")]
    public void GivenThereIsConfigurationForMinimumPlayers(int minimum, int max)
    {
        _optionsMock.Setup(x => x.Value).Returns(new PlayerConfiguration
        {
            MinimumPlayersAllowed = minimum,
            MaximumPlayersAllowed = max,
            StartingBalance = 0
        });
    }

    [Given("the amount of computer players set to be generated is {int}")]
    public void GivenAmountOfGeneratedPlayersIs(int generatedCount)
    {
        _numberGeneratorMock.Setup(x => x.GenerateNewRandomNumber(
            _optionsMock.Object.Value.MinimumPlayersAllowed,
            _optionsMock.Object.Value.MaximumPlayersAllowed + 1)).Returns(generatedCount);

        _expectedComputerPlayersGenerated = new Core.Entities.Player[generatedCount];
        for (int i = 0; i < generatedCount; i++)
        {
            _expectedComputerPlayersGenerated[i] = new Core.Entities.Player($"Player {i + 2}", 10);
            _playerGeneratorMock.Setup(x => x.GenerateNewPlayer(_expectedComputerPlayersGenerated[i].Name)).Returns(_expectedComputerPlayersGenerated[i]);
        }
    }

    [Given("a human player is set to be generated")]
    public void GivenHumanPlayerGenerated()
    {
        _expectedHumanPlayer = new Core.Entities.Player("Player 1", 10);
        _playerGeneratorMock.Setup(x => x.GenerateNewPlayer(_expectedHumanPlayer.Name)).Returns(_expectedHumanPlayer);
    }

    [When("CreateComputerPlayers is called")]
    public void WhenCreateComputerPlayersCalled()
    {
        IPlayerFactory playerFactory = new Logic.Players.PlayerFactory(
            _optionsMock.Object,
            _playerGeneratorMock.Object,
            _numberGeneratorMock.Object);

        _actualComputerPlayers = playerFactory.CreateComputerPlayers();
    }

    [When("CreateHumanPlayer is called")]
    public void WhenCreateHumanPlayerCalled()
    {
        IPlayerFactory playerFactory = new Logic.Players.PlayerFactory(
            _optionsMock.Object,
            _playerGeneratorMock.Object,
            _numberGeneratorMock.Object);

        _actualHumanPlayer = playerFactory.CreateHumanPlayer();
    }

    [Then("{int} players would have been generated")]
    public void ThenPlayersWouldHaveBeenGenerated(int generatedCount)
    {
        _numberGeneratorMock.Verify(x => x.GenerateNewRandomNumber(
            _optionsMock.Object.Value.MinimumPlayersAllowed,
            _optionsMock.Object.Value.MaximumPlayersAllowed + 1), Times.Once);

        _playerGeneratorMock.Verify(x => x.GenerateNewPlayer(It.IsAny<string>()), Times.Exactly(generatedCount));

        for (int i = 0; i < generatedCount; i++)
        {
            _playerGeneratorMock.Verify(x => x.GenerateNewPlayer(_expectedComputerPlayersGenerated![i].Name), Times.Once);
        }

        Assert.AreEqual(_expectedComputerPlayersGenerated!.Length, _actualComputerPlayers!.Count());
        Assert.IsTrue(_expectedComputerPlayersGenerated.SequenceEqual(_actualComputerPlayers!));
    }

    [Then("the human player would have been generated")]
    public void ThenHumanPlayerGenerated()
    {
        _playerGeneratorMock.Verify(x => x.GenerateNewPlayer(_expectedHumanPlayer!.Name), Times.Once);
        Assert.AreEqual(_expectedHumanPlayer, _actualHumanPlayer);
    }
}