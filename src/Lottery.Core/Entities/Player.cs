namespace Lottery.Core.Entities;

public class Player(string name, decimal balance)
{
    public string Name { get; } = name;
    public decimal Balance { get; private set; } = balance;
}