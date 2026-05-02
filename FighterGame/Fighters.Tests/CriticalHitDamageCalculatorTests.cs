using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class CriticalHitDamageCalculatorTests
{
    private const int BaseDamage = 50;

    private const double HalfChance = 0.5;

    private const double DoubleFactor = 2.0;

    private class FixedDamageCalculator : IDamageCalculator
    {
        private readonly int _value;

        public FixedDamageCalculator( int value ) => _value = value;
        public int Calculate( IFighter attacker, IFighter defender ) => _value;
    }

    [Test]
    public void Calculate_RollBelowChance_AppliesMultiplier()
    {
        FakeRandom random = new FakeRandom( 0.0 );
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            new FixedDamageCalculator( BaseDamage ),
            random,
            chance: HalfChance,
            multiplier: DoubleFactor
        );

        ( IFighter attacker, IFighter defender ) = Utils.MakePair();

        int damage = calc.Calculate( attacker, defender );

        Assert.That( damage, Is.EqualTo( BaseDamage * DoubleFactor ) );
    }

    [Test]
    public void Calculate_RollAboveChance_KeepsBaseDamage()
    {
        FakeRandom random = new FakeRandom( 0.99 );
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            new FixedDamageCalculator( BaseDamage ), random, chance: 0.15, multiplier: DoubleFactor
        );

        ( IFighter attacker, IFighter defender ) = Utils.MakePair();

        int damage = calc.Calculate( attacker, defender );

        Assert.That( damage, Is.EqualTo( BaseDamage ) );
    }

    [Test]
    public void Constructor_ChanceOutOfRange_Throws()
    {
        Assert.That(
            () => new CriticalHitDamageCalculator(
                new FixedDamageCalculator( 1 ),
                new Random(),
                chance: 1.5
            ),
            Throws.TypeOf<ArgumentOutOfRangeException>()
        );
    }

    [Test]
    public void Constructor_MultiplierLessThanOne_Throws()
    {
        Assert.That(
            () => new CriticalHitDamageCalculator(
                new FixedDamageCalculator( 1 ),
                new Random(),
                multiplier: 0.5
            ),
            Throws.TypeOf<ArgumentOutOfRangeException>()
        );
    }
}