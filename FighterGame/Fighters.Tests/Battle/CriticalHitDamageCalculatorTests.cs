using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests.Battle;

public class CriticalHitDamageCalculatorTests
{
    private const int BaseDamage = 50;

    private static Mock<IDamageCalculator> CalculatorReturning( int value )
    {
        Mock<IDamageCalculator> calculator = new Mock<IDamageCalculator>();
        calculator.Setup( c => c.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) ).Returns( value );

        return calculator;
    }

    private static Random RandomReturning( double roll )
    {
        Mock<Random> random = new Mock<Random>();
        random.Setup( r => r.NextDouble() ).Returns( roll );

        return random.Object;
    }

    private static int Calculate( CriticalHitDamageCalculator calc ) => calc.Calculate(
        FighterBuilder.CreateMock().Object,
        FighterBuilder.CreateMock().Object
    );

    [Fact]
    public void Calculate_RollBelowChance_AppliesMultiplier()
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( BaseDamage ).Object,
            RandomReturning( 0.0 ),
            criticalChance: 0.5,
            criticalMultiplier: 2.0
        );

        int critDamage = Calculate( calc );

        Assert.Equal( BaseDamage * 2, critDamage );
    }

    [Fact]
    public void Calculate_RollAboveChance_KeepsBaseDamage()
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( BaseDamage ).Object,
            RandomReturning( 0.99 ),
            criticalChance: 0.15,
            criticalMultiplier: 2.0
        );

        int critDamage = Calculate( calc );

        Assert.Equal( BaseDamage, critDamage );
    }

    [Fact]
    public void Calculate_RollEqualsChance_IsNotCritical()
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( BaseDamage ).Object,
            RandomReturning( 0.15 ),
            criticalChance: 0.15,
            criticalMultiplier: 2.0
        );

        int critDamage = Calculate( calc );

        Assert.Equal( BaseDamage, critDamage );
    }

    [Fact]
    public void Calculate_CriticalMultiplier_RoundsToNearestInt()
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( 5 ).Object,
            RandomReturning( 0.0 ),
            criticalChance: 1.0,
            criticalMultiplier: 1.5
        );

        int critDamage = Calculate( calc ); // 5 * 1.5 = 7.5 -> 8

        Assert.Equal( 8, critDamage );
    }

    [Theory]
    [InlineData( 0.0 )]
    [InlineData( 0.5 )]
    [InlineData( 0.999 )]
    public void Calculate_ZeroChance_NeverCrits( double roll )
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( BaseDamage ).Object,
            RandomReturning( roll ),
            criticalChance: 0.0,
            criticalMultiplier: 2.0
        );

        int critDamage = Calculate( calc );

        Assert.Equal( BaseDamage, critDamage );
    }

    [Fact]
    public void Calculate_ChanceOne_AlwaysCrits()
    {
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            CalculatorReturning( BaseDamage ).Object,
            RandomReturning( 0.999 ),
            criticalChance: 1.0,
            criticalMultiplier: 2.0
        );

        int critDamage = Calculate( calc );

        Assert.Equal( 100, critDamage );
    }

    [Fact]
    public void Calculate_Always_DelegatesToInnerCalculatorOnce()
    {
        Mock<IDamageCalculator> mockedCalculator = CalculatorReturning( BaseDamage );
        CriticalHitDamageCalculator calc = new CriticalHitDamageCalculator(
            mockedCalculator.Object,
            RandomReturning( 0.99 ),
            criticalChance: 0.15,
            criticalMultiplier: 2.0
        );

        Calculate( calc );

        mockedCalculator.Verify( c => c.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ), Times.Once );
    }

    [Theory]
    [InlineData( -0.1 )]
    [InlineData( 1.1 )]
    public void Constructor_ChanceOutOfRange_ThrowsArgumentOutOfRangeException( double chance )
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new CriticalHitDamageCalculator(
                CalculatorReturning( 1 ).Object,
                RandomReturning( 0 ),
                criticalChance: chance
            )
        );
    }

    [Fact]
    public void Constructor_MultiplierLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new CriticalHitDamageCalculator(
                CalculatorReturning( 1 ).Object,
                RandomReturning( 0 ),
                criticalMultiplier: 0.5
            )
        );
    }
}