using Fighters.Commands;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class RemoveFighterConsoleCommandTests
{
    [Fact]
    public void Execute_ValidIndex_RemovesFighterAndAnnounces()
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterMother.CreateDefault( "A" ) );
        roster.Add( FighterMother.CreateDefault( "B" ) );
        Mock<IConsole> console = new Mock<IConsole>();
        console
            .Setup( c => c.ReadLine() )
            .Returns( "1" );

        RemoveFighterConsoleCommand sut = new RemoveFighterConsoleCommand( roster, console.Object );

        // Act
        sut.Execute();

        // Assert
        Assert.Single( roster.Fighters );
        Assert.Equal( "B", roster.Fighters[ 0 ].Name );
    }

    [Theory]
    [InlineData( "0" )]
    [InlineData( "3" )]
    [InlineData( "-1" )]
    [InlineData( "abc" )]
    [InlineData( "" )]
    public void Execute_InvalidIndex_LeavesRosterIntact( string input )
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterMother.CreateDefault( "A" ) );
        roster.Add( FighterMother.CreateDefault( "B" ) );
        Mock<IConsole> console = new Mock<IConsole>();
        console
            .Setup( c => c.ReadLine() )
            .Returns( input );

        RemoveFighterConsoleCommand sut = new RemoveFighterConsoleCommand( roster, console.Object );

        // Act
        sut.Execute();

        // Assert
        Assert.Equal( 2, roster.Fighters.Count );
    }
}