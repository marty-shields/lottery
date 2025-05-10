using Lottery.Core.Entities;

namespace Lottery.Core.Interfaces.Players;

public interface IPlayerGenerator
{
    /// <summary>
    /// This will generate a new player based on the name which is provided.
    /// The starting balance for the player will be generated using the configured starting balance
    /// </summary>
    /// <param name="name">The unique name to be provided to the player</param>
    /// <returns>A new instance of the <see cref="Player"/> record</returns>
    Player GenerateNewPlayer(string name);
}
