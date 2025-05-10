using Lottery.Core.Interfaces.Random;
using Lottery.Logic.Random;

using Reqnroll;

namespace Lottery.Logic.Tests.Random;

[Binding]
[Scope(Feature = "Random Number Generator")]
public class RandomNumberGeneratorSteps
{
    private int min;
    private int max;
    private int generatedNumber;
    private Exception? exceptionThrown;

    [Given(@"the minimum number is (.*)")]
    public void GivenTheMinimumNumberIs(int min)
    {
        this.min = min;
    }

    [Given(@"the maximum number is (.*)")]
    public void GivenTheMaximumNumberIs(int max)
    {
        this.max = max;
    }

    [When(@"I generate a random number")]
    public void WhenIGenerateARandomNumber()
    {
        IRandomNumberGenerator random = new RandomNumberGenerator();
        generatedNumber = random.GenerateNewRandomNumber(min, max);
    }

    [When(@"I attempt to generate a random number")]
    public void WhenIAttemptToGenerateARandomNumber()
    {
        IRandomNumberGenerator random = new RandomNumberGenerator();
        try
        {
            generatedNumber = random.GenerateNewRandomNumber(min, max + 1);
        }
        catch (Exception ex)
        {
            this.exceptionThrown = ex;
        }
    }

    [Then(@"the result should be between 1 and 99 inclusive")]
    public void ThenTheGeneratedNumberShouldBeBetweenTheMinimumAndMaximum()
    {
        Assert.IsTrue(generatedNumber >= 1 && generatedNumber < 100);
    }

    [Then(@"the result should always be 42")]
    public void ThenResultShouldAlwaysBe42()
    {
        Assert.AreEqual(42, generatedNumber);
    }

    [Then(@"an exception should be thrown indicating an invalid range")]
    public void ThenAnExceptionShouldBeThrown()
    {
        Assert.IsNotNull(exceptionThrown);
        Assert.IsInstanceOfType(exceptionThrown, typeof(ArgumentOutOfRangeException));
    }
}