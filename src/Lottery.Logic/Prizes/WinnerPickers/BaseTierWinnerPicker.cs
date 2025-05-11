using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Random;
using Lottery.Logic.Tickets;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes.WinnerPickers;

public abstract class BaseTierWinnerPicker : BaseWinnerPicker
{
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public BaseTierWinnerPicker(IRandomNumberGenerator randomNumberGenerator, IOptions<TicketConfiguration> ticketConfigurationOptions) : base(ticketConfigurationOptions)
    {
        _randomNumberGenerator = randomNumberGenerator;
    }

    private protected (IEnumerable<Ticket> tierWinners, decimal tierAmount) CalculateTierWinners(
        IReadOnlyList<Ticket> totalLotteryTickets,
        IReadOnlyList<Ticket> ticketsThatCanWin,
        decimal winnersCountPercentage,
        decimal winnersPayoutPercentage)
    {
        List<Ticket> tierWinners = new List<Ticket>();
        List<Ticket> ticketPool = ticketsThatCanWin.ToList();
        decimal tierAmount = 0;
        decimal tierWinnersCount = Math.Round(totalLotteryTickets.Count() * winnersCountPercentage);
        if (tierWinnersCount > 0)
        {
            tierAmount = Math.Round(CalculateTotalPrizeMoney(totalLotteryTickets) * winnersPayoutPercentage / tierWinnersCount, 2);
            for (int i = 0; i < tierWinnersCount; i++)
            {
                Ticket tierWinner = ticketPool.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketPool.Count));
                tierWinners.Add(tierWinner);
                ticketPool.RemoveTicketFromTicketPool(tierWinner);
            }
        }

        return (tierWinners, tierAmount);
    }
}
