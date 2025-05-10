namespace Lottery.Core.Interfaces.Random;

public interface IRandomNumberGenerator
{
    /// <summary>
    /// This will generate a new random number based on the min and max parameters specified
    /// </summary>
    /// <param name="minValue">Inclusive lower bound</param>
    /// <param name="maxValue">Exclusive upper bound</param>
    /// <returns>A random number between min and max</returns>
    /// // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     minValue is greater than maxValue.
    int GenerateNewRandomNumber(int minValue, int maxValue);
}
