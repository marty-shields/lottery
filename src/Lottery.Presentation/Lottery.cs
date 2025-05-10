using Lottery.Core.Configuration;
using Lottery.Core.Entities;
using Lottery.Core.Interfaces.Players;
using Lottery.Core.Interfaces.Prizes;
using Lottery.Core.Interfaces.Random;
using Lottery.Core.Interfaces.Tickets;

using Microsoft.Extensions.Options;

namespace Lottery.Presentation;

public class Lottery
{
    private readonly IOptions<TicketConfiguration> _ticketOptions;
    private readonly IPlayerFactory _playerFactory;
    private readonly ITicketGenerator _ticketGenerator;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly IPrizeGenerator _prizeGenerator;

    public Lottery(
        IOptions<TicketConfiguration> ticketOptions,
        IPlayerFactory playerFactory,
        ITicketGenerator ticketGenerator,
        IRandomNumberGenerator randomNumberGenerator,
        IPrizeGenerator prizeGenerator)
    {
        _ticketOptions = ticketOptions;
        _playerFactory = playerFactory;
        _ticketGenerator = ticketGenerator;
        _randomNumberGenerator = randomNumberGenerator;
        _prizeGenerator = prizeGenerator;
    }

    public void Run()
    {
        Player humanPlayer = _playerFactory.CreateHumanPlayer();
        IEnumerable<Player> computerPlayers = _playerFactory.CreateComputerPlayers();
        List<Ticket> ticketsPurchased = new List<Ticket>();

        Console.WriteLine($"Welcome to the Bede Lottery, {humanPlayer.Name}");
        Console.WriteLine($"* Your digital balance is: {humanPlayer.Balance}");
        Console.WriteLine($"* Ticket price: {_ticketOptions.Value.TicketPrice:C}");
        Console.WriteLine();

        ticketsPurchased.AddRange(GenerateTicketsForHuman(humanPlayer));
        ticketsPurchased.AddRange(GenerateTicketsForCPUPlayers(computerPlayers));
        Console.WriteLine();
        Console.WriteLine($"{computerPlayers.Count()} other CPU players have also purchased tickets");
        Console.WriteLine("Tickets purchased:");
        Console.WriteLine($"Total: {ticketsPurchased.Count}");
        foreach (var tickets in ticketsPurchased.GroupBy(x => x.PlayerName))
        {
            Console.WriteLine($"{tickets.Key}: {tickets.Count()}");
        }

        var prizeResults = _prizeGenerator.GeneratePrizes(ticketsPurchased);
        Console.WriteLine();
        Console.WriteLine("Ticket draw results:");
        Console.WriteLine($"* Grand prize: {prizeResults.GrandPrizeTicket!.PlayerName} wins {prizeResults.GrandPrizeAmount:C}");
        Console.WriteLine($"* Second tier: {GeneratePrizeResultString(prizeResults.SecondTierPrizeTickets)} wins {prizeResults.SecondTierPrizeAmount:C} each");
        Console.WriteLine($"* Third tier: {GeneratePrizeResultString(prizeResults.ThirdTierPrizeTickets)} wins {prizeResults.ThirdTierPrizeAmount:C} each");
        Console.WriteLine("Congratulations to the winners!");
        Console.WriteLine($"House profit: {prizeResults.HouseProfit:C}");
    }

    private string GeneratePrizeResultString(IEnumerable<Ticket> winningTickets)
    {
        IOrderedEnumerable<IGrouping<string, Ticket>> winnersGrouped = winningTickets
            .GroupBy(t => t.PlayerName)
            .OrderBy(t => t.Key);
        return string.Join(", ", winnersGrouped.Select(g => $"{g.Key}{(g.Count() > 1 ? $"(x{g.Count()})" : string.Empty)}"));
    }

    private IEnumerable<Ticket> GenerateTicketsForCPUPlayers(IEnumerable<Player> computerPlayers)
    {
        List<Ticket> totalTickets = new List<Ticket>();
        foreach (var player in computerPlayers)
        {
            int ticketsToPurchase = _randomNumberGenerator
                .GenerateNewRandomNumber(
                    _ticketOptions.Value.MinimumTicketsPerPlayer,
                    _ticketOptions.Value.MaximumTicketsPerPlayer + 1
                );

            totalTickets.AddRange(_ticketGenerator.GenerateTicketsForPlayer(player, ticketsToPurchase));
        }

        return totalTickets;
    }

    private IEnumerable<Ticket> GenerateTicketsForHuman(Player humanPlayer)
    {
        List<Ticket> totalTickets = new List<Ticket>();
        bool successfulInput = false;
        while (!successfulInput)
        {
            Console.WriteLine($"How many tickets would you like to buy {humanPlayer.Name}?");
            successfulInput = int.TryParse(Console.ReadLine(), out int humanPlayerTicketsRequested);
            if (successfulInput)
            {
                try
                {
                    totalTickets.AddRange(_ticketGenerator.GenerateTicketsForPlayer(humanPlayer, humanPlayerTicketsRequested));
                    AdjustPlayerBalance(humanPlayer, totalTickets.Count);

                    if (totalTickets.Count < humanPlayerTicketsRequested)
                    {
                        Console.WriteLine($"Warning, {humanPlayerTicketsRequested} tickets were requested however only {totalTickets.Count} were purchased");
                    }
                }
                catch (InvalidOperationException)
                {
                    successfulInput = false;
                }
            }

            if (!successfulInput)
            {
                Console.WriteLine("Invalid input provided");
            }
        }

        return totalTickets;
    }

    private void AdjustPlayerBalance(Player player, int requestedTicketsCount)
    {
        double ticketPrice = _ticketOptions.Value.TicketPrice;
        double totalCost = requestedTicketsCount * ticketPrice;
        player.DeductBalance(totalCost);
    }
}
