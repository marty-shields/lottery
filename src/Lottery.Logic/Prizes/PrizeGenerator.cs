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

    public PrizeGenerator(IRandomNumberGenerator randomNumberGenerator, IOptions<TicketConfiguration> ticketConfiguration)
    {
        _randomNumberGenerator = randomNumberGenerator;
        _ticketConfiguration = ticketConfiguration;
    }

    public Prize GeneratePrizes(IEnumerable<Ticket> tickets)
    {
        Prize prize = new();
        double totalPrizeMoney = tickets.Count() * _ticketConfiguration.Value.TicketPrice;
        List<Ticket> ticketsThatCanWin = [.. tickets];

        CalculateGrandPrizeWinner(prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateSecondTierWinners(tickets, prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateThirdTierWinners(tickets, prize, totalPrizeMoney, ticketsThatCanWin);
        CalculateHouseProfit(prize, totalPrizeMoney);

        return prize;
    }

    private void CalculateHouseProfit(Prize prize, double totalPrizeMoney)
    {
        double grandPrizeTotal = prize.GrandPrizeAmount;
        double secondTierTotal = prize.SecondTierPrizeAmount * prize.SecondTierPrizeTickets.Count();
        double thirdTierTotal = prize.ThirdTierPrizeAmount * prize.ThirdTierPrizeTickets.Count();

        prize.HouseProfit = Math.Round(totalPrizeMoney - (grandPrizeTotal + secondTierTotal + thirdTierTotal), 2);
    }

    private void CalculateThirdTierWinners(IEnumerable<Ticket> tickets, Prize prize, double totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        double thirdTierWinnerCount = Math.Round(tickets.Count() * 0.2);
        if (thirdTierWinnerCount > 0)
        {
            prize.ThirdTierPrizeAmount = Math.Round(totalPrizeMoney * 0.1 / thirdTierWinnerCount, 2);
            for (int i = 0; i < thirdTierWinnerCount; i++)
            {
                Ticket thirdTierWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
                ((List<Ticket>)prize.ThirdTierPrizeTickets).Add(thirdTierWinner);
                _ = ticketsThatCanWin.Remove(thirdTierWinner);
            }
        }
    }

    private void CalculateSecondTierWinners(IEnumerable<Ticket> tickets, Prize prize, double totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        double secondTierWinnerCount = Math.Round(tickets.Count() * 0.1);
        if (secondTierWinnerCount > 0)
        {
            prize.SecondTierPrizeAmount = Math.Round(totalPrizeMoney * 0.3 / secondTierWinnerCount, 2);
            for (int i = 0; i < secondTierWinnerCount; i++)
            {
                Ticket secondTierWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
                ((List<Ticket>)prize.SecondTierPrizeTickets).Add(secondTierWinner);
                _ = ticketsThatCanWin.Remove(secondTierWinner);
            }
        }
    }

    private void CalculateGrandPrizeWinner(Prize prize, double totalPrizeMoney, List<Ticket> ticketsThatCanWin)
    {
        Ticket grandPrizeWinner = ticketsThatCanWin.ElementAt(_randomNumberGenerator.GenerateNewRandomNumber(0, ticketsThatCanWin.Count));
        prize.GrandPrizeTicket = grandPrizeWinner;
        prize.GrandPrizeAmount = Math.Round(totalPrizeMoney * 0.5, 2);
        _ = ticketsThatCanWin.Remove(grandPrizeWinner);
    }
}
