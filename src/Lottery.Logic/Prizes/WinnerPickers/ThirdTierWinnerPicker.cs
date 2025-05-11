using System;

using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes.WinnerPickers;

public class ThirdTierWinnerPicker : BaseTierWinnerPicker
{
    private readonly decimal _thirdPrizeWinnerCountPercentage;
    private readonly decimal _thirdPrizePayoutPercentage;

    public ThirdTierWinnerPicker(
        IRandomNumberGenerator randomNumberGenerator, 
        IOptions<PrizeConfiguration> prizeConfigurationOptions,
        IOptions<TicketConfiguration> ticketConfigurationOptions) : base(randomNumberGenerator,ticketConfigurationOptions)
    {
        _thirdPrizeWinnerCountPercentage = prizeConfigurationOptions.Value.ThirdPrizeWinnerCountPercentage;
        _thirdPrizePayoutPercentage = prizeConfigurationOptions.Value.ThirdPrizePayoutPercentage;
    }

    public override WinnerPickerType Type => WinnerPickerType.ThirdTier;

    public override (IEnumerable<Ticket> prizeWinners, decimal prizeAmount) CalculateWinners(IReadOnlyList<Ticket> totalLotteryTickets, IReadOnlyList<Ticket> ticketsThatCanWin)
    {
        return CalculateTierWinners(totalLotteryTickets, ticketsThatCanWin, _thirdPrizeWinnerCountPercentage, _thirdPrizePayoutPercentage);
    }
}
