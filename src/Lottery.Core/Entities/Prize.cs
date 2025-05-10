namespace Lottery.Core.Entities;

public class Prize
{
    public Ticket? GrandPrizeTicket { get; set; }
    public double GrandPrizeAmount { get; set; } = 0;
    public IEnumerable<Ticket> SecondTierPrizeTickets { get; set; } = new List<Ticket>();
    public double SecondTierPrizeAmount { get; set; } = 0;
    public IEnumerable<Ticket> ThirdTierPrizeTickets { get; set; } = new List<Ticket>();
    public double ThirdTierPrizeAmount { get; set; } = 0;
    public double HouseProfit { get; set; } = 0;
}
