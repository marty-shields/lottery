using Lottery.Core.Interfaces.Random;

namespace Lottery.Logic.Random;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly System.Random _random;

    public RandomNumberGenerator()
    {
        _random = new System.Random();
    }

    public RandomNumberGenerator(int seed)
    {
        _random = new System.Random(seed);
    }

    public int GenerateNewRandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }
}
