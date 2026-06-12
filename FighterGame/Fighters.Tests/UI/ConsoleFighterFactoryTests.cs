using Fighters.Models.Fighters;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.UI;

public class ConsoleFighterFactoryTests
{
    [Fact]
    public void Create_ValidInput_BuildsFighterFromSelections()
    {
        // Arrange
        Mock<IConsole> console = new Mock<IConsole>();
        console.SetupSequence( c => c.ReadLine() )
            .Returns( "Hero" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" );

        ConsoleFighterFactory sut = new ConsoleFighterFactory(
            console.Object,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        // Act
        IFighter fighter = sut.Create();

        // Assert
        Assert.Equal( "Hero", fighter.Name );
        Assert.Equal( 150, fighter.MaxHealth );
        Assert.Equal( 26, fighter.Damage );
        Assert.Equal( 0, fighter.Armor );
        Assert.Equal( 5, fighter.Initiative );
    }

    [Fact]
    public void Create_EmptyNameEntered_KeepsAskingUntilValidName()
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

        ConsoleFighterFactory sut = new ConsoleFighterFactory(
            console.Object,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        // Act
        IFighter fighter = sut.Create();

        // Assert
        Assert.Equal( "Hero", fighter.Name );
        console.Verify( c => c.ReadLine(), Times.Exactly( 7 ) );
    }

    [Fact]
    public void Create_InvalidOptionEntered_KeepsAskingUntilValidOption()
    {
        // Arrange
        Mock<IConsole> console = new Mock<IConsole>();
        console.SetupSequence( c => c.ReadLine() )
            .Returns( "Hero" )
            .Returns( "abc" )
            .Returns( "99" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" )
            .Returns( "1" );

        ConsoleFighterFactory sut = new ConsoleFighterFactory(
            console.Object,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        // Act
        IFighter fighter = sut.Create();

        // Assert
        Assert.Equal( "Hero", fighter.Name );
        Assert.Equal( 150, fighter.MaxHealth );
        console.Verify( c => c.ReadLine(), Times.Exactly( 7 ) );
    }
}