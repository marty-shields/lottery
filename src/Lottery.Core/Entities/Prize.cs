namespace Lottery.Core.Entities;

public class Prize
{
    public Ticket? GrandPrizeTicket { get; set; }
    public decimal GrandPrizeAmount { get; set; } = 0;
    public IList<Ticket> SecondTierPrizeTickets { get; set; } = new List<Ticket>();
    public decimal SecondTierPrizeAmount { get; set; } = 0;
    public IList<Ticket> ThirdTierPrizeTickets { get; set; } = new List<Ticket>();
    public decimal ThirdTierPrizeAmount { get; set; } = 0;
    public decimal HouseProfit { get; set; } = 0;
}
