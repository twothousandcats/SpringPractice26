using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests.Battle;

public class CriticalHitDamageCalculatorTests
{
    [Fact]
    public void Calculate_RollBelowChance_AppliesMultiplier()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 50 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.0 );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 0.5,
            criticalMultiplier: 2.0
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 100, actualDamage );
    }

    [Fact]
    public void Calculate_RollAboveChance_KeepsBaseDamage()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 50 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.99 );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 0.15,
            criticalMultiplier: 2.0
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 50, actualDamage );
    }

    [Fact]
    public void Calculate_RollEqualsChance_IsNotCritical()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 50 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.15 );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 0.15,
            criticalMultiplier: 2.0
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 50, actualDamage );
    }

    [Fact]
    public void Calculate_FractionalMultiplier_UsesBankersRoundingToEven()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 5 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.0 );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 1.0,
            criticalMultiplier: 1.5
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 8, actualDamage );
    }

    [Theory]
    [InlineData( 0.0 )]
    [InlineData( 0.5 )]
    [InlineData( 0.999 )]
    public void Calculate_ZeroCriticalChance_KeepsBaseDamageForAnyRoll( double roll )
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 50 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( roll );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 0.0,
            criticalMultiplier: 2.0
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 50, actualDamage );
    }

    [Fact]
    public void Calculate_ChanceOne_AlwaysCrits()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 50 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.999 );

        CriticalHitDamageCalculator sut = new CriticalHitDamageCalculator(
            damageCalculator.Object,
            random.Object,
            criticalChance: 1.0,
            criticalMultiplier: 2.0
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 100, actualDamage );
    }

    [Theory]
    [InlineData( -0.1 )]
    [InlineData( 1.1 )]
    public void Constructor_ChanceOutOfRange_ThrowsArgumentOutOfRangeException( double chance )
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        Mock<Random> random = new Mock<Random>();

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new CriticalHitDamageCalculator(
                damageCalculator.Object,
                random.Object,
                criticalChance: chance
            )
        );
    }

    [Fact]
    public void Constructor_MultiplierLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        Mock<Random> random = new Mock<Random>();

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new CriticalHitDamageCalculator(
                damageCalculator.Object,
                random.Object,
                criticalMultiplier: 0.5
            )
        );
    }
}