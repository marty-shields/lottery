using Lottery.Core.Configuration;
using Lottery.Core.Entities;

using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;

using Microsoft.Extensions.Options;

namespace Lottery.Logic.Prizes;

public class PrizeGenerator : IPrizeGenerator
{
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly TicketConfiguration _ticketConfiguration;
    private readonly PrizeConfiguration _prizeConfiguration;

    public PrizeGenerator(
        IRandomNumberGenerator randomNumberGenerator,
        IOptions<TicketConfiguration> ticketConfiguration,
        IOptions<PrizeConfiguration> prizeConfiguration)
    {
        _randomNumberGenerator = randomNumberGenerator;
        _ticketConfiguration = ticketConfiguration.Value;
        _prizeConfiguration = prizeConfiguration.Value;
    }

    public Prize GeneratePrizes(IEnumerable<Ticket> tickets)
    {
        decimal totalPrizeMoney = tickets.Count() * _ticketConfiguration.TicketPrice;
        List<Ticket> ticketsThatCanWin = [.. tickets];

        (Ticket grandPrizeWinner, decimal grandPrizeAmount) = CalculateGrandPrizeWinner(totalPrizeMoney, ticketsThatCanWin);
        (IEnumerable<Ticket> secondTierWinners, decimal secondTierAmount) = CalculateTierWinners(tickets, totalPrizeMoney, ticketsThatCanWin, _prizeConfiguration.SecondPrizeWinnerCountPercentage, _prizeConfiguration.SecondPrizePayoutPercentage);
        (IEnumerable<Ticket> thirdTierWinners, decimal thirdTierAmount) = CalculateTierWinners(tickets, totalPrizeMoney, ticketsThatCanWin, _prizeConfiguration.ThirdPrizeWinnerCountPercentage, _prizeConfiguration.ThirdPrizePayoutPercentage);

        decimal houseProfit = CalculateHouseProfit(totalPrizeMoney, grandPrizeAmount, secondTierWinners, secondTierAmount, thirdTierWinners, thirdTierAmount);

        return new Prize(grandPrizeWinner, grandPrizeAmount, secondTierWinners, secondTierAmount, thirdTierWinners, thirdTierAmount, houseProfit);
    }

    private decimal CalculateHouseProfit(
        decimal totalPrizeMoney,
        decimal grandPrizeAmount,
        IEnumerable<Ticket> secondTierWinners,
        decimal secondTierAmount,
        IEnumerable<Ticket> thirdTierWinners,
        decimal thirdTierAmount)
    {
        decimal secondTierTotal = secondTierAmount * secondTierWinners.Count();
        decimal thirdTierTotal = thirdTierAmount * thirdTierWinners.Count();

        return Math.Round(totalPrizeMoney - (grandPrizeAmount + secondTierTotal + thirdTierTotal), 2);
    }

    private (IEnumerable<Ticket> tierWinners, decimal tierAmount) CalculateTierWinners(
        IEnumerable<Ticket> tickets,
        decimal totalPrizeMoney,
        List<Ticket> ticketsThatCanWin,
        decimal winnersCountPercentage,
        decimal winnersPayoutPercentage)
    {
        List<Ticket> tierWinners = new List<Ticket>();
        decimal tierAmount = 0;
        decimal tierWinnersCount = Math.Round(tickets.Count() * winnersCountPercentage);
        if (tierWinnersCount > 0)
        {
            tierAmount = Math.Round(totalPrizeMoney * winnersPayoutPercentage / tierWinnersCount, 2);
            for (int i = 0; i < tierWinnersCount; i++)
            {
                Ticket tierWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
                tierWinners.Add(tierWinner);
                RemoveWinningTicketFromPool(ticketsThatCanWin, tierWinner);
            }
        }

        return (tierWinners, tierAmount);
    }

    private (Ticket grandPrizeWinner, decimal grandPrizeAmount) CalculateGrandPrizeWinner(decimal totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        Ticket grandPrizeWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
        decimal grandPrizeAmount = Math.Round(totalPrizeMoney * _prizeConfiguration.GrandPrizePayoutPercentage, 2);
        RemoveWinningTicketFromPool(ticketsThatCanWin, grandPrizeWinner);
        return (grandPrizeWinner, grandPrizeAmount);
    }

    private void RemoveWinningTicketFromPool(List<Ticket> ticketsThatCanWin, Ticket winningTicket)
    {
        ticketsThatCanWin.Remove(winningTicket);
    }
}
