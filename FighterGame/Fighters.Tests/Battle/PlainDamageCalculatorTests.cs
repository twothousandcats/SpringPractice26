using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Battle;

public class PlainDamageCalculatorTests
{
    private readonly PlainDamageCalculator _calculator = new PlainDamageCalculator();

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

        // Act
        int dealtDamage = _calculator.Calculate( attacker, defender );

        // Assert
        Assert.Equal( 5, dealtDamage );
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

        // Act
        int dealtDamage = _calculator.Calculate( attacker, defender );

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

        // Act
        int dealtDamage = _calculator.Calculate( attacker, defender );

        // Assert
        Assert.Equal( 0, dealtDamage );
    }
}