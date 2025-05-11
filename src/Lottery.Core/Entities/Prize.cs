namespace Lottery.Core.Entities;

public record Prize(
    Ticket GrandPrizeTicket,
    decimal GrandPrizeAmount,
    IEnumerable<Ticket> SecondTierPrizeTickets,
    decimal SecondTierPrizeAmount,
    IEnumerable<Ticket> ThirdTierPrizeTickets,
    decimal ThirdTierPrizeAmount,
    decimal HouseProfit);
