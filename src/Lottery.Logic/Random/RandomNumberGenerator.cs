using Lottery.Core.Interfaces.Random;

namespace Lottery.Logic.Random;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly System.Random _random;

    public RandomNumberGenerator()
    {
        _random = new System.Random();
    }

    public int GenerateNewRandomNumber(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}
