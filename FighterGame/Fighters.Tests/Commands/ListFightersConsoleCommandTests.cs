using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class ListFightersConsoleCommandTests
{
    [Fact]
    public void Execute_EmptyRoster_PrintsEmptyMessage()
    {
        Mock<IConsole> console = new Mock<IConsole>();
        ListFightersConsoleCommand command = new ListFightersConsoleCommand( new FighterRoster(), console.Object );

        command.Execute();

        console.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "empty" ) ) ), Times.Once );
    }

    [Fact]
    public void Execute_NonEmptyRoster_PrintsNumberedDescriptions()
    {
        FighterRoster roster = new FighterRoster();
        Mock<IFighter> first = FighterBuilder.CreateMock( "A" );
        first.SetupGet( f => f.Description ).Returns( "A the brave" );
        Mock<IFighter> second = FighterBuilder.CreateMock( "B" );
        second.SetupGet( f => f.Description ).Returns( "B the bold" );
        roster.Add( first.Object );
        roster.Add( second.Object );
        Mock<IConsole> console = new Mock<IConsole>();
        ListFightersConsoleCommand command = new ListFightersConsoleCommand( roster, console.Object );

        command.Execute();

        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.StartsWith( "1. " ) && s.Contains( "A the brave" ) ) ),
            Times.Once
        );

        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.StartsWith( "2. " ) && s.Contains( "B the bold" ) ) ),
            Times.Once
        );
    }
}