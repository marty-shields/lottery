namespace Lottery.Core.Entities;

public class Ticket(string playerName)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string PlayerName { get; } = playerName;
}
