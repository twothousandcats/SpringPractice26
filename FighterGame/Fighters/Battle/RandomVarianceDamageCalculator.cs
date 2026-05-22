using Fighters.Models.Fighters;

namespace Fighters.Battle;

public class RandomVarianceDamageCalculator : IDamageCalculator
{
    private const double MinFactor = 0.8;

    private const double MaxFactor = 1.1;

    private readonly IDamageCalculator _damageCalculator;

    private readonly Random _random;

    public RandomVarianceDamageCalculator( IDamageCalculator damageCalculator, Random random )
    {
        _damageCalculator = damageCalculator;
        _random = random;
    }

    public int Calculate( IFighter attacker, IFighter defender )
    {
        int baseDamage = _damageCalculator.Calculate( attacker, defender );
        double factor = MinFactor + ( _random.NextDouble() * ( MaxFactor - MinFactor ) );

        return ( int )Math.Round( baseDamage * factor );
    }
}