using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests.Battle;

public class RandomVarianceDamageCalculatorTests
{
    private const int BaseDamage = 100;

    private static RandomVarianceDamageCalculator Create(
        int baseDamage,
        double roll,
        out Mock<IDamageCalculator> damageCalculator
    )
    {
        damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator.Setup( calc => calc.Calculate(
                It.IsAny<IFighter>(),
                It.IsAny<IFighter>()
            )
        ).Returns( baseDamage );

        Mock<Random> random = new Mock<Random>();
        random.Setup( r => r.NextDouble() ).Returns( roll );

        return new RandomVarianceDamageCalculator( damageCalculator.Object, random.Object );
    }

    [Theory]
    [InlineData( 0.0, 80 )]
    [InlineData( 0.5, 95 )]
    [InlineData( 0.9999999, 110 )]
    public void Calculate_GivenRoll_ScalesBaseDamageWithinVariance( double roll, int expected )
    {
        RandomVarianceDamageCalculator calc = Create( BaseDamage, roll, out _ );

        int damage = calc.Calculate(
            FighterBuilder.CreateMock().Object,
            FighterBuilder.CreateMock().Object
        );

        Assert.Equal( expected, damage );
    }

    [Fact]
    public void Calculate_ZeroBaseDamage_ReturnsZero()
    {
        RandomVarianceDamageCalculator calc = Create( 0, 0.5, out _ );

        int damage = calc.Calculate(
            FighterBuilder.CreateMock().Object,
            FighterBuilder.CreateMock().Object
        );

        Assert.Equal( 0, damage );
    }

    [Fact]
    public void Calculate_Always_DelegatesToCalculatorOnce()
    {
        IFighter attacker = FighterBuilder.CreateMock().Object;
        IFighter defender = FighterBuilder.CreateMock().Object;
        RandomVarianceDamageCalculator calc = Create( BaseDamage, 0.5, out Mock<IDamageCalculator> calculator );

        calc.Calculate( attacker, defender );

        calculator.Verify( c => c.Calculate( attacker, defender ), Times.Once );
    }
}