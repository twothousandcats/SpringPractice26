namespace Casino.Infrastructure;

public sealed class RandomGenerator : IRandomGenerator
{
    private readonly Random _random;

    public RandomGenerator() : this(new Random())
    {
    }

    public RandomGenerator(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        _random = random;
    }

    public int NextInclusive(int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException("min must be less than or equal to max");
        }
        
        return _random.Next(min, max + 1);
    }
}