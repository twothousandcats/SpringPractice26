using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests.Battle;

public class RandomVarianceDamageCalculatorTests
{
    [Theory]
    [InlineData( 0.0, 80 )]
    [InlineData( 0.5, 95 )]
    [InlineData( 0.9999999, 110 )]
    public void Calculate_GivenRoll_ScalesBaseDamageWithinVariance( double roll, int expectedDamage )
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 100 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( roll );

        RandomVarianceDamageCalculator sut = new RandomVarianceDamageCalculator(
            damageCalculator.Object, random.Object
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( expectedDamage, actualDamage );
    }

    [Fact]
    public void Calculate_ZeroBaseDamage_ReturnsZero()
    {
        // Arrange
        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 0 );

        Mock<Random> random = new Mock<Random>();
        random
            .Setup( rng => rng.NextDouble() )
            .Returns( 0.5 );

        RandomVarianceDamageCalculator sut = new RandomVarianceDamageCalculator(
            damageCalculator.Object,
            random.Object
        );

        // Act
        int actualDamage = sut.Calculate( new TestFighter(), new TestFighter() );

        // Assert
        Assert.Equal( 0, actualDamage );
    }
}