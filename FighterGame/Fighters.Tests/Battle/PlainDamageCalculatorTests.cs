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
        IFighter attacker = FighterBuilder.CreateMock( damage: 20 ).Object;
        IFighter defender = FighterBuilder.CreateMock( armor: 5 ).Object;

        int dealtDamage = _calculator.Calculate( attacker, defender );

        Assert.Equal( 15, dealtDamage );
    }

    [Fact]
    public void Calculate_ArmorGreaterThanDamage_ReturnsZero()
    {
        IFighter attacker = FighterBuilder.CreateMock( damage: 5 ).Object;
        IFighter defender = FighterBuilder.CreateMock( armor: 20 ).Object;

        int dealtDamage = _calculator.Calculate( attacker, defender );

        Assert.Equal( 0, dealtDamage );
    }

    [Fact]
    public void Calculate_ArmorEqualsDamage_ReturnsZero()
    {
        IFighter attacker = FighterBuilder.CreateMock( damage: 10 ).Object;
        IFighter defender = FighterBuilder.CreateMock( armor: 10 ).Object;

        int dealtDamage = _calculator.Calculate( attacker, defender );

        Assert.Equal( 0, dealtDamage );
    }
}