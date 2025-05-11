using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes.WinnerPickers;

public class GrandPrizeWinnerPicker : BaseWinnerPicker
{
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly PrizeConfiguration _prizeConfiguration;

    public GrandPrizeWinnerPicker(
        IRandomNumberGenerator randomNumberGenerator,
        IOptions<PrizeConfiguration> prizeConfigurationOptions,
        IOptions<TicketConfiguration> ticketConfigurationOptions) : base(ticketConfigurationOptions)
    {
        _randomNumberGenerator = randomNumberGenerator;
        _prizeConfiguration = prizeConfigurationOptions.Value;
    }

    public override WinnerPickerType Type => WinnerPickerType.GrandPrize;

    public override (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) CalculateWinners(IReadOnlyList<Ticket> totalLotteryTickets, IReadOnlyList<Ticket> ticketsThatCanWin)
    {
        Ticket grandPrizeWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
        decimal grandPrizeAmount = Math.Round(CalculateTotalPrizeMoney(totalLotteryTickets) * _prizeConfiguration.GrandPrizePayoutPercentage, 2);
        return ([grandPrizeWinner], grandPrizeAmount);
    }
}
