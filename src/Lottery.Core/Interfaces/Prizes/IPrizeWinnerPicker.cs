using Lottery.Core.Entities;

namespace Lottery.Core.Interfaces.Prizes;

public  interface IPrizeWinnerPicker
{
    /// <summary>
    /// The type of winner picker
    /// </summary>
    public WinnerPickerType Type { get; }

    /// <summary>
    /// This will calculate the winners from a list of tickets that can win. Winners are determined based on the actual implementation
    /// </summary>
    /// <param name="totalLotteryTickets">All of the tickets in the lottery</param>
    /// <param name="ticketsThatCanWin">The remaining tickets that can still win</param>
    /// <returns></returns>
    (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) CalculateWinners(IReadOnlyList<Ticket> totalLotteryTickets, IReadOnlyList<Ticket> ticketsThatCanWin);
}