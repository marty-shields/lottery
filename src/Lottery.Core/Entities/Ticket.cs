namespace Lottery.Core.Entities;

public class Ticket
{
    public Ticket(string playerName)
    {
        Id = Guid.NewGuid();
        PlayerName = playerName;
    }

    public Guid Id { get; }
    public string PlayerName { get; }
}
