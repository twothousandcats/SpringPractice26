using Fighters.Battle;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class PlainDamageCalculatorTests
{
    private static Fighter Make( string name )
    {
        return new Fighter( name, new Human(), new Knight(), new Fists(), new NoArmor() );
    }

    [Test]
    public void Calculate_ReturnsCorrectDamageValue()
    {
        PlainDamageCalculator calculator = new PlainDamageCalculator();
        Fighter attacker = Make( "A" );
        Fighter defender = Make( "B" );

        int damage = calculator.Calculate( attacker, defender );

        Assert.That( damage, Is.EqualTo( 7 ) );
    }

    [Test]
    public void Calculate_ReturnsZeroWhenArmonGreaterThanDamage()
    {
        PlainDamageCalculator calculator = new PlainDamageCalculator();
        Fighter attacker = Make( "A" );
        Fighter defender = new Fighter( "B", new Dwarf(), new Knight(), new Fists(), new PlateArmor() );

        int damage = calculator.Calculate( attacker, defender );

        Assert.That( damage, Is.EqualTo( 0 ) );
    }
}