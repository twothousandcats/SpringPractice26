namespace Casino.Infrastructure;

public sealed class RandomGenerator : IRandomGenerator
{
    private readonly Random _random;

    public RandomGenerator() : this( new Random() )
    {
    }

    private RandomGenerator( Random random )
    {
        _random = random;
    }

    public int NextInclusive( int min, int max )
    {
        if ( min > max )
        {
            throw new ArgumentException( "Минимальное должно быть меньше или равно максимальному" );
        }

        return _random.Next( min, max + 1 );
    }
}