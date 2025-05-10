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
    private readonly IOptions<PlayerConfiguration> _options;
    private readonly IPlayerGenerator _playerGenerator;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public PlayerFactory(
        IOptions<PlayerConfiguration> options,
        IPlayerGenerator playerGenerator,
        IRandomNumberGenerator randomNumberGenerator)
    {
        _options = options;
        _playerGenerator = playerGenerator;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public IEnumerable<Player> CreateComputerPlayers()
    {
        int cpuPlayerCount = _randomNumberGenerator.GenerateNewRandomNumber(_options.Value.MinimumPlayersAllowed, _options.Value.MaximumPlayersAllowed + 1);
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
