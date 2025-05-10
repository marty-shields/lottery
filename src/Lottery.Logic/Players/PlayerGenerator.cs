using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Players;

using Microsoft.Extensions.Options;


namespace Lottery.Logic.Players;

public class PlayerGenerator : IPlayerGenerator
{
    private readonly IOptions<PlayerConfiguration> _options;

    public PlayerGenerator(IOptions<PlayerConfiguration> options)
    {
        _options = options;
    }

    public Player GenerateNewPlayer(string name) => new(name, _options.Value.StartingBalance);
}
