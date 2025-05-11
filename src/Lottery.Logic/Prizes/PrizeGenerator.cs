using System.Collections.Immutable;

using Lottery.Core.Configuration;
using Lottery.Core.Entities;

using Lottery.Core.Interfaces.Prizes;
using Lottery.Logic.Tickets;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes;

public class PrizeGenerator : IPrizeGenerator
{
    private readonly IPrizeWinnerPickerFactory _prizeWinnerPickerFactory;
    private readonly TicketConfiguration _ticketConfiguration;

    public PrizeGenerator(
        IPrizeWinnerPickerFactory prizeWinnerPickerFactory,
        IOptions<TicketConfiguration> ticketConfiguration)
    {
        _prizeWinnerPickerFactory = prizeWinnerPickerFactory;
        _ticketConfiguration = ticketConfiguration.Value;
    }

    public Prize GeneratePrizes(IEnumerable<Ticket> tickets)
    {
        List<Ticket> ticketsThatCanWin = [.. tickets];

        IPrizeWinnerPicker prizeWinnerPicker = _prizeWinnerPickerFactory.GetPrizeWinnerPicker(WinnerPickerType.GrandPrize);
        (IEnumerable<Ticket> grandPrizeWinners, decimal grandPrizeAmount) = prizeWinnerPicker.CalculateWinners(tickets.ToImmutableList(), ticketsThatCanWin);
        ticketsThatCanWin.RemoveTicketsFromTicketPool(grandPrizeWinners);

        prizeWinnerPicker = _prizeWinnerPickerFactory.GetPrizeWinnerPicker(WinnerPickerType.SecondTier);
        (IEnumerable<Ticket> secondTierWinners, decimal secondTierAmount) = prizeWinnerPicker.CalculateWinners(tickets.ToImmutableList(), ticketsThatCanWin);
        ticketsThatCanWin.RemoveTicketsFromTicketPool(secondTierWinners);
        

        prizeWinnerPicker = _prizeWinnerPickerFactory.GetPrizeWinnerPicker(WinnerPickerType.ThirdTier);
        (IEnumerable<Ticket> thirdTierWinners, decimal thirdTierAmount) = prizeWinnerPicker.CalculateWinners(tickets.ToImmutableList(), ticketsThatCanWin);
        ticketsThatCanWin.RemoveTicketsFromTicketPool(secondTierWinners);

        decimal houseProfit = CalculateHouseProfit(tickets, grandPrizeAmount, secondTierWinners, secondTierAmount, thirdTierWinners, thirdTierAmount);

        return new Prize(grandPrizeWinners.First(), grandPrizeAmount, secondTierWinners, secondTierAmount, thirdTierWinners, thirdTierAmount, houseProfit);
    }

    private decimal CalculateHouseProfit(
        IEnumerable<Ticket> totalTickets,
        decimal grandPrizeAmount,
        IEnumerable<Ticket> secondTierWinners,
        decimal secondTierAmount,
        IEnumerable<Ticket> thirdTierWinners,
        decimal thirdTierAmount)
    {
        decimal totalPrizeMoney = totalTickets.Count() * _ticketConfiguration.TicketPrice;
        decimal secondTierTotal = secondTierAmount * secondTierWinners.Count();
        decimal thirdTierTotal = thirdTierAmount * thirdTierWinners.Count();

        return Math.Round(totalPrizeMoney - (grandPrizeAmount + secondTierTotal + thirdTierTotal), 2);
    }
}
