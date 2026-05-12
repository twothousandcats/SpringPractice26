using Fighters.Models.Fighters;

namespace Fighters.Battle;

public class CriticalHitDamageCalculator : IDamageCalculator
{
    private readonly IDamageCalculator _baseCalculator;

    private readonly Random _random;

    private readonly double _criticalChance;

    private readonly double _criticalMultiplier;

    public CriticalHitDamageCalculator(
        IDamageCalculator baseCalculator,
        Random random,
        double criticalChance = 0.15,
        double criticalMultiplier = 2.0
    )
    {
        if ( criticalChance is < 0 or > 1 )
        {
            throw new ArgumentOutOfRangeException( nameof( criticalChance ), "Chance must be in [0, 1]" );
        }

        if ( criticalMultiplier < 1 )
        {
            throw new ArgumentOutOfRangeException( nameof( criticalMultiplier ), "Multiplier must be >= 1" );
        }

        _baseCalculator = baseCalculator;
        _random = random;
        _criticalChance = criticalChance;
        _criticalMultiplier = criticalMultiplier;
    }

    public int Calculate( IFighter attacker, IFighter defender )
    {
        int baseDamage = _baseCalculator.Calculate( attacker, defender );
        bool isCrit = _random.NextDouble() < _criticalChance;
        return isCrit
            ? ( int )Math.Round( baseDamage * _criticalMultiplier )
            : baseDamage;
    }
}