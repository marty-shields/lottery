using Lottery.Core.Configuration;
using Lottery.Core.Entities;

using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes;

public class PrizeGenerator : IPrizeGenerator
{
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly IOptions<TicketConfiguration> _ticketConfiguration;
    private readonly IOptions<PrizeConfiguration> _prizeConfiguration;

    public PrizeGenerator(
        IRandomNumberGenerator randomNumberGenerator,
        IOptions<TicketConfiguration> ticketConfiguration,
        IOptions<PrizeConfiguration> prizeConfiguration)
    {
        _randomNumberGenerator = randomNumberGenerator;
        _ticketConfiguration = ticketConfiguration;
        _prizeConfiguration = prizeConfiguration;
    }

    public Prize GeneratePrizes(IEnumerable<Ticket> tickets)
    {
        Prize prize = new();
        decimal totalPrizeMoney = tickets.Count() * _ticketConfiguration.Value.TicketPrice;
        List<Ticket> ticketsThatCanWin = [.. tickets];

        CalculateGrandPrizeWinner(prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateSecondTierWinners(tickets, prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateThirdTierWinners(tickets, prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateHouseProfit(prize, totalPrizeMoney);

        return prize;
    }

    private void CalculateHouseProfit(Prize prize, decimal totalPrizeMoney)
    {
        decimal grandPrizeTotal = prize.GrandPrizeAmount;
        decimal secondTierTotal = prize.SecondTierPrizeAmount * prize.SecondTierPrizeTickets.Count;
        decimal thirdTierTotal = prize.ThirdTierPrizeAmount * prize.ThirdTierPrizeTickets.Count;

        prize.HouseProfit = Math.Round(totalPrizeMoney - (grandPrizeTotal + secondTierTotal + thirdTierTotal), 2);
    }

    private void CalculateThirdTierWinners(IEnumerable<Ticket> tickets, Prize prize, decimal totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        decimal thirdTierWinnerCount = Math.Round(tickets.Count() * _prizeConfiguration.Value.ThirdPrizeWinnerCountPercentage);
        if (thirdTierWinnerCount > 0)
        {
            prize.ThirdTierPrizeAmount = Math.Round(totalPrizeMoney * _prizeConfiguration.Value.ThirdPrizePayoutPercentage / thirdTierWinnerCount, 2);
            for (int i = 0; i < thirdTierWinnerCount; i++)
            {
                Ticket thirdTierWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
                prize.ThirdTierPrizeTickets.Add(thirdTierWinner);
                _ = ticketsThatCanWin.Remove(thirdTierWinner);
            }
        }
    }

    private void CalculateSecondTierWinners(IEnumerable<Ticket> tickets, Prize prize, decimal totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        decimal secondTierWinnerCount = Math.Round(tickets.Count() * _prizeConfiguration.Value.SecondPrizeWinnerCountPercentage);
        if (secondTierWinnerCount > 0)
        {
            prize.SecondTierPrizeAmount = Math.Round(totalPrizeMoney * _prizeConfiguration.Value.SecondPrizePayoutPercentage / secondTierWinnerCount, 2);
            for (int i = 0; i < secondTierWinnerCount; i++)
            {
                Ticket secondTierWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
                prize.SecondTierPrizeTickets.Add(secondTierWinner);
                _ = ticketsThatCanWin.Remove(secondTierWinner);
            }
        }
    }

    private void CalculateGrandPrizeWinner(Prize prize, decimal totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        Ticket grandPrizeWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
        prize.GrandPrizeTicket = grandPrizeWinner;
        prize.GrandPrizeAmount = Math.Round(totalPrizeMoney * _prizeConfiguration.Value.GrandPrizePayoutPercentage, 2);
        _ = ticketsThatCanWin.Remove(grandPrizeWinner);
    }
}
