using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Battle;

public class PlainDamageCalculatorTests
{
    [Fact]
    public void Calculate_DamageGreaterThanArmor_ReturnsDifference()
    {
        // Arrange
        IFighter attacker = new TestFighter
        {
            Damage = 20
        };

        IFighter defender = new TestFighter
        {
            Armor = 5
        };

        PlainDamageCalculator sut = new PlainDamageCalculator();

        // Act
        int dealtDamage = sut.Calculate( attacker, defender );

        // Assert
        Assert.Equal( 15, dealtDamage );
    }

    [Fact]
    public void Calculate_ArmorGreaterThanDamage_ReturnsZero()
    {
        // Arrange
        IFighter attacker = new TestFighter
        {
            Damage = 5
        };

        IFighter defender = new TestFighter
        {
            Armor = 20
        };

        PlainDamageCalculator sut = new PlainDamageCalculator();

        // Act
        int dealtDamage = sut.Calculate( attacker, defender );

        // Assert
        Assert.Equal( 0, dealtDamage );
    }

    [Fact]
    public void Calculate_ArmorEqualsDamage_ReturnsZero()
    {
        // Arrange
        IFighter attacker = new TestFighter
        {
            Damage = 20
        };

        IFighter defender = new TestFighter
        {
            Armor = 20
        };

        PlainDamageCalculator sut = new PlainDamageCalculator();

        // Act
        int dealtDamage = sut.Calculate( attacker, defender );

        // Assert
        Assert.Equal( 0, dealtDamage );
    }
}