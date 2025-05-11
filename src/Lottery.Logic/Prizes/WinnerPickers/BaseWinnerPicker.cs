using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes.WinnerPickers;

public abstract class BaseWinnerPicker : IPrizeWinnerPicker
{
    private readonly decimal _ticketPrice;

    public BaseWinnerPicker(IOptions<TicketConfiguration> ticketConfigurationOptions)
    {
        _ticketPrice = ticketConfigurationOptions.Value.TicketPrice;
    }

    public abstract WinnerPickerType Type { get; }

    public abstract (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) CalculateWinners(IReadOnlyList<Ticket> totalLotteryTickets, IReadOnlyList<Ticket> ticketsThatCanWin);

    private protected decimal CalculateTotalPrizeMoney(IReadOnlyList<Ticket> totalLotteryTickets) => totalLotteryTickets.Count() * _ticketPrice;
}
