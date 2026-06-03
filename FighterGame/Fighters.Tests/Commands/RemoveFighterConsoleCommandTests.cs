using Fighters.Commands;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class RemoveFighterConsoleCommandTests
{
    private static FighterRoster RosterWith( params string[] names )
    {
        FighterRoster roster = new FighterRoster();
        foreach ( string name in names )
        {
            roster.Add( FighterBuilder.CreateMock( name ).Object );
        }

        return roster;
    }

    [Fact]
    public void Execute_ValidIndex_RemovesFighterAndAnnounces()
    {
        FighterRoster roster = RosterWith( "A", "B" );
        Mock<IConsole> console = new Mock<IConsole>();
        console.Setup( c => c.ReadLine() ).Returns( "1" );
        RemoveFighterConsoleCommand command = new RemoveFighterConsoleCommand( roster, console.Object );

        command.Execute();

        Assert.Single( roster.Fighters );
        Assert.Equal( "B", roster.Fighters[ 0 ].Name );
        console.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "Removed" ) ) ), Times.Once );
    }

    [Theory]
    [InlineData( "0" )]
    [InlineData( "3" )]
    [InlineData( "-1" )]
    [InlineData( "abc" )]
    [InlineData( "" )]
    public void Execute_InvalidIndex_LeavesRosterIntact( string input )
    {
        FighterRoster roster = RosterWith( "A", "B" );
        Mock<IConsole> console = new Mock<IConsole>();
        console.Setup( c => c.ReadLine() ).Returns( input );
        RemoveFighterConsoleCommand command = new RemoveFighterConsoleCommand( roster, console.Object );

        command.Execute();

        Assert.Equal( 2, roster.Fighters.Count );
        console.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "Invalid" ) ) ), Times.Once );
    }
}