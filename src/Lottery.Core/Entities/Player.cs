namespace Lottery.Core.Entities;

public class Player
{
    public Player(string name, double balance)
    {
        Name = name;
        Balance = balance;
    }

    public string Name { get; }
    public double Balance { get; private set; }

    public void AddBalance(double amount)
    {
        Balance += amount;
    }

    public void DeductBalance(double amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
    }
}