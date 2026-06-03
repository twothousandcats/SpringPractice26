using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace Fighters.Test.TestData;

public static class FighterBuilder
{
    public static Fighter CreateHero( string name = "Hero" ) => new Fighter(
        name, new Human(), new Knight(), new Fists(), new NoArmor()
    );

    public static Mock<IFighter> CreateMock(
        string name = "Fighter",
        int currentHealth = 100,
        int maxHealth = 100,
        int damage = 10,
        int armor = 0,
        int initiative = 0,
        bool isAlive = true
    )
    {
        var fighter = new Mock<IFighter>();
        fighter.SetupGet( f => f.Name ).Returns( name );
        fighter.SetupGet( f => f.CurrentHealth ).Returns( currentHealth );
        fighter.SetupGet( f => f.MaxHealth ).Returns( maxHealth );
        fighter.SetupGet( f => f.Damage ).Returns( damage );
        fighter.SetupGet( f => f.Armor ).Returns( armor );
        fighter.SetupGet( f => f.Initiative ).Returns( initiative );
        fighter.SetupGet( f => f.IsAlive ).Returns( isAlive );

        return fighter;
    }
}