using Lottery.Core.Entities;

namespace Lottery.Core.Interfaces.Players;

public interface IPlayerFactory
{
    /// <summary>
    /// This method is used to create computer players.
    /// The number of players created is random and will be between the minimum and maximum values set in the configuration.
    /// The starting balance will be set based on the configuration settings.
    /// </summary>
    /// <returns>A collection of computer players.</returns>
    IEnumerable<Player> CreateComputerPlayers();

    /// <summary>
    /// This method is used to create a human player.
    /// It will use configuration settings to setup the starting balance.
    /// </summary>
    Player CreateHumanPlayer();
}
