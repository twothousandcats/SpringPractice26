using Fighters.Models.Fighters;
using Fighters.Tests.Helpers;
using Fighters.UI;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class ConsoleFighterFactoryTests
{
    [Test]
    public void Create_ReadsValidInput_ProducesFighter()
    {
        FakeConsole console = new FakeConsole( "Hero", "1", "1", "1", "1" );
        ConsoleFighterFactory factory = new ConsoleFighterFactory(
            console,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        IFighter fighter = factory.Create();

        Assert.That( fighter.Name, Is.EqualTo( "Hero" ) );
        Assert.That( fighter.MaxHealth, Is.GreaterThan( 0 ) );
    }

    [Test]
    public void Create_RetriesOnEmptyName()
    {
        FakeConsole console = new FakeConsole( "", "  ", "Hero", "1", "1", "1", "1" );
        ConsoleFighterFactory factory = new ConsoleFighterFactory(
            console,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        IFighter fighter = factory.Create();

        Assert.That( fighter.Name, Is.EqualTo( "Hero" ) );
        Assert.That( console.Output, Has.Some.Contains( "cant be empty" ) );
    }

    [Test]
    public void Create_RetriesOnInvalidOption()
    {
        FakeConsole console = new FakeConsole( "Hero", "abc", "99", "1", "1", "1", "1" );
        ConsoleFighterFactory factory = new ConsoleFighterFactory(
            console,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        IFighter fighter = factory.Create();

        Assert.That( fighter, Is.Not.Null );
        Assert.That( console.Output, Has.Some.Contains( "Invalid option" ) );
    }
}