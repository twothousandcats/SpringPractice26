using Fighters.Models.Fighters;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.UI;

public class ConsoleFighterFactoryTests
{
    private static ConsoleFighterFactory Create( IConsole console ) => new ConsoleFighterFactory(
        console,
        FighterCatalog.Armors,
        FighterCatalog.Classes,
        FighterCatalog.Races,
        FighterCatalog.Weapons
    );

    [Fact]
    public void Create_ValidInput_BuildsFighterFromSelections()
    {
        Mock<IConsole> console = new Mock<IConsole>();
        console.SetupSequence( c => c.ReadLine() )
            .Returns( "Hero" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" );

        IFighter fighter = Create( console.Object ).Create();

        Assert.Equal( "Hero", fighter.Name );
        Assert.Contains( "Human", fighter.Description );
        Assert.Contains( "Knight", fighter.Description );
        Assert.Contains( "Axe", fighter.Description );
        Assert.Contains( "No Armor", fighter.Description );
    }

    [Fact]
    public void Create_EmptyNameEntered_RepromptsUntilValid()
    {
        Mock<IConsole> console = new Mock<IConsole>();
        console.SetupSequence( c => c.ReadLine() )
            .Returns( "" )
            .Returns( "   " )
            .Returns( "Hero" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" );

        IFighter fighter = Create( console.Object ).Create();

        Assert.Equal( "Hero", fighter.Name );
        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.Contains( "cant be empty" ) ) ),
            Times.AtLeast( 2 )
        );
    }

    [Fact]
    public void Create_InvalidOptionEntered_RepromptsUntilValid()
    {
        Mock<IConsole> console = new Mock<IConsole>();
        console.SetupSequence( c => c.ReadLine() )
            .Returns( "Hero" )
            .Returns( "abc" )
            .Returns( "99" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" );

        IFighter fighter = Create( console.Object ).Create();

        Assert.NotNull( fighter );
        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.Contains( "Invalid option" ) ) ),
            Times.AtLeast( 2 )
        );
    }
}