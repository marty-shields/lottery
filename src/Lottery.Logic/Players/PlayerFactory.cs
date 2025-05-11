using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Players;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Players;

public class PlayerFactory : IPlayerFactory
{
    private const string PlayerNamePrefix = "Player";
    private const string HumanPlayerName = $"{PlayerNamePrefix} 1";
    private readonly PlayerConfiguration _playerConfiguration;
    private readonly IPlayerGenerator _playerGenerator;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public PlayerFactory(
        IOptions<PlayerConfiguration> options,
        IPlayerGenerator playerGenerator,
        IRandomNumberGenerator randomNumberGenerator)
    {
        _playerConfiguration = options.Value;
        _playerGenerator = playerGenerator;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public IEnumerable<Player> CreateComputerPlayers()
    {
        int cpuPlayerCount = _randomNumberGenerator.GenerateNewRandomNumber(_playerConfiguration.MinimumPlayersAllowed, _playerConfiguration.MaximumPlayersAllowed + 1);
        Player[] playersToCreate = new Player[cpuPlayerCount];
        for (int i = 0; i < cpuPlayerCount; i++)
        {
            int cpuPlayerNumber = i + 2;
            playersToCreate[i] = _playerGenerator.GenerateNewPlayer($"{PlayerNamePrefix} {cpuPlayerNumber}");
        }
        return playersToCreate;
    }

    public Player CreateHumanPlayer()
    {
        return _playerGenerator.GenerateNewPlayer(HumanPlayerName);
    }
}
