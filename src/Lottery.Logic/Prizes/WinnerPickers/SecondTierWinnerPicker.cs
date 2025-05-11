using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes.WinnerPickers;

public class SecondTierWinnerPicker : BaseTierWinnerPicker
{
    private readonly decimal _secondPrizeWinnerCountPercentage;
    private readonly decimal _secondPrizePayoutPercentage;

    public SecondTierWinnerPicker(
        IRandomNumberGenerator randomNumberGenerator,
        IOptions<PrizeConfiguration> prizeConfigurationOptions,
        IOptions<TicketConfiguration> ticketConfigurationOptions) : base(randomNumberGenerator, ticketConfigurationOptions)
    {
        _secondPrizeWinnerCountPercentage = prizeConfigurationOptions.Value.SecondPrizeWinnerCountPercentage;
        _secondPrizePayoutPercentage = prizeConfigurationOptions.Value.SecondPrizePayoutPercentage;
    }

    public override WinnerPickerType Type => WinnerPickerType.SecondTier;

    public override (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) CalculateWinners(IReadOnlyList<Ticket> totalLotteryTickets, IReadOnlyList<Ticket> ticketsThatCanWin)
    {
        return CalculateTierWinners(totalLotteryTickets, ticketsThatCanWin, _secondPrizeWinnerCountPercentage, _secondPrizePayoutPercentage);
    }
}
