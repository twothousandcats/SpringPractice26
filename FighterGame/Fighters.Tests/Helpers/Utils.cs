using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Tests.Helpers;

public static class Utils
{
    public static IFighter CreateFighter( string name ) =>
        new Fighter( name, new Human(), new Knight(), new Fists(), new NoArmor() );

    public static (IFighter attacker, IFighter defender) MakePair()
    {
        IFighter a = CreateFighter( "A" );
        IFighter b = CreateFighter( "B" );
        return ( a, b );
    }
}