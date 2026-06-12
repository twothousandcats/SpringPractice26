using Fighters.Commands;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class ListFightersConsoleCommandTests
{
    [Fact]
    public void Execute_EmptyRoster_WritesSingleNotice()
    {
        // Arrange
        Mock<IConsole> console = new Mock<IConsole>();
        ListFightersConsoleCommand command = new ListFightersConsoleCommand( new FighterRoster(), console.Object );

        // Act
        command.Execute();

        // Assert
        console.Verify( c => c.WriteLine( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public void Execute_NonEmptyRoster_WritesLinePerFighter()
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterMother.CreateDefault( "First Fighter" ) );
        roster.Add( FighterMother.CreateDefault( "Second Fighter" ) );
        Mock<IConsole> console = new Mock<IConsole>();
        ListFightersConsoleCommand sut = new ListFightersConsoleCommand( roster, console.Object );

        // Act
        sut.Execute();

        // Assert
        console.Verify( c => c.WriteLine( It.IsAny<string>() ), Times.Exactly( 2 ) );
    }
}