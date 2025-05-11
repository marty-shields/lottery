namespace Lottery.Core.Entities;

public record Ticket(string playerName)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string PlayerName { get; } = playerName;
}
