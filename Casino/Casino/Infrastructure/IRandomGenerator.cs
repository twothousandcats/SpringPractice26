namespace Casino.Infrastructure;

public interface IRandomGenerator
{
    // Returns a value in the inclusive range
    int NextInclusive(int min, int max);
}