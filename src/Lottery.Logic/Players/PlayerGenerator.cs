using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Players;

using Microsoft.Extensions.Options;


namespace Lottery.Logic.Players;

public class PlayerGenerator : IPlayerGenerator
{
    private readonly PlayerConfiguration _playerConfiguration;

    public PlayerGenerator(IOptions<PlayerConfiguration> options)
    {
        _playerConfiguration = options.Value;
    }

    public Player GenerateNewPlayer(string name) => new(name, _playerConfiguration.StartingBalance);
}
